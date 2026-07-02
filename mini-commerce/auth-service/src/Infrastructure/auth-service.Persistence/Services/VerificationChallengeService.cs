using auth_service.Application.Abstractions.Services;
using auth_service.Application.Exceptions;
using auth_service.Domain.Entities;
using auth_service.Domain.Enums;
using auth_service.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace auth_service.Persistence.Services
{
    public class VerificationChallengeService : IVerificationChallengeService
    {
        private readonly AuthServiceDbContext _context;
        private readonly IConfiguration _configuration;

        public VerificationChallengeService(AuthServiceDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> CreateCodeAsync(Guid userId, VerificationPurpose purpose, string? targetEmail, Guid correlationId, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var normalizedTargetEmail = NormalizeEmail(targetEmail);
            var activeChallenges = await _context.VerificationChallenges
                .Where(challenge => challenge.UserId == userId && challenge.Purpose == purpose && challenge.TargetEmail == normalizedTargetEmail && challenge.ConsumedAt == null && challenge.ExpiresAt > now)
                .ToListAsync(cancellationToken);

            foreach (var activeChallenge in activeChallenges)
            {
                activeChallenge.ConsumedAt = now;
                activeChallenge.UpdateAt = now;
            }

            var code = GenerateCode();
            var challenge = new VerificationChallenge
            {
                UserId = userId,
                Purpose = purpose,
                TargetEmail = normalizedTargetEmail,
                CodeHash = HashCode(userId, purpose, normalizedTargetEmail, code),
                ExpiresAt = now.AddMinutes(GetExpirationMinutes()),
                MaxAttempts = GetMaxAttempts(),
                CreatedAt = now,
                UpdateAt = now,
                LastSentAt = now,
                isActive = true,
                isDeleted = false,
                CorrelationId = correlationId
            };

            await _context.VerificationChallenges.AddAsync(challenge, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return code;
        }

        public async Task ValidateCodeAsync(Guid userId, VerificationPurpose purpose, string? targetEmail, string code, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new BadRequestException("Doğrulama kodu boş olamaz.");
            }

            var now = DateTime.UtcNow;
            var normalizedTargetEmail = NormalizeEmail(targetEmail);
            var challenge = await _context.VerificationChallenges
                .Where(challenge => challenge.UserId == userId && challenge.Purpose == purpose && challenge.TargetEmail == normalizedTargetEmail && challenge.ConsumedAt == null && !challenge.isDeleted)
                .OrderByDescending(challenge => challenge.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (challenge == null || challenge.ExpiresAt <= now)
            {
                throw new BadRequestException("Doğrulama kodu geçersiz veya süresi dolmuş.");
            }

            if (challenge.AttemptCount >= challenge.MaxAttempts)
            {
                throw new TooManyRequestsException("Doğrulama kodu için maksimum deneme hakkı aşıldı.");
            }

            var expectedHash = HashCode(userId, purpose, normalizedTargetEmail, code.Trim());
            if (!CryptographicOperations.FixedTimeEquals(Convert.FromHexString(challenge.CodeHash), Convert.FromHexString(expectedHash)))
            {
                challenge.AttemptCount++;
                challenge.UpdateAt = now;
                await _context.SaveChangesAsync(cancellationToken);
                throw new BadRequestException("Doğrulama kodu hatalı.");
            }

            challenge.ConsumedAt = now;
            challenge.UpdateAt = now;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task ValidateEmailChangeCodesAsync(Guid userId, string oldEmail, string newEmail, string oldEmailCode, string newEmailCode, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(oldEmailCode) || string.IsNullOrWhiteSpace(newEmailCode))
            {
                throw new BadRequestException("Doğrulama kodları boş olamaz.");
            }

            var now = DateTime.UtcNow;
            var normalizedOldEmail = NormalizeEmail(oldEmail);
            var normalizedNewEmail = NormalizeEmail(newEmail);
            var oldEmailChallenge = await GetActiveChallengeAsync(userId, VerificationPurpose.EmailChangeOld, normalizedOldEmail, cancellationToken);
            var newEmailChallenge = await GetActiveChallengeAsync(userId, VerificationPurpose.EmailChangeNew, normalizedNewEmail, cancellationToken);

            EnsureChallengeIsUsable(oldEmailChallenge, now);
            EnsureChallengeIsUsable(newEmailChallenge, now);

            var oldEmailCodeMatches = IsCodeMatch(oldEmailChallenge, userId, VerificationPurpose.EmailChangeOld, normalizedOldEmail, oldEmailCode.Trim());
            var newEmailCodeMatches = IsCodeMatch(newEmailChallenge, userId, VerificationPurpose.EmailChangeNew, normalizedNewEmail, newEmailCode.Trim());

            if (!oldEmailCodeMatches || !newEmailCodeMatches)
            {
                MarkFailedAttempt(oldEmailChallenge, oldEmailCodeMatches, now);
                MarkFailedAttempt(newEmailChallenge, newEmailCodeMatches, now);
                await _context.SaveChangesAsync(cancellationToken);
                throw new BadRequestException("Doğrulama kodları hatalı.");
            }

            oldEmailChallenge.ConsumedAt = now;
            oldEmailChallenge.UpdateAt = now;
            newEmailChallenge.ConsumedAt = now;
            newEmailChallenge.UpdateAt = now;
            await _context.SaveChangesAsync(cancellationToken);
        }

        private static string GenerateCode()
        {
            var value = RandomNumberGenerator.GetInt32(0, 1_000_000);
            return value.ToString("D6");
        }

        private string HashCode(Guid userId, VerificationPurpose purpose, string? targetEmail, string code)
        {
            var secret = _configuration["VerificationCode:Secret"];
            if (string.IsNullOrWhiteSpace(secret))
            {
                secret = _configuration["Token:SecurityKey"];
            }

            if (string.IsNullOrWhiteSpace(secret))
            {
                throw new InvalidOperationException("VerificationCode:Secret veya Token:SecurityKey yapılandırması tanımlı olmalıdır.");
            }

            var payload = $"{userId}:{purpose}:{targetEmail ?? string.Empty}:{code}";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            return Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(payload)));
        }

        private async Task<VerificationChallenge> GetActiveChallengeAsync(Guid userId, VerificationPurpose purpose, string? targetEmail, CancellationToken cancellationToken)
        {
            var challenge = await _context.VerificationChallenges
                .Where(challenge => challenge.UserId == userId && challenge.Purpose == purpose && challenge.TargetEmail == targetEmail && challenge.ConsumedAt == null && !challenge.isDeleted)
                .OrderByDescending(challenge => challenge.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (challenge == null)
            {
                throw new BadRequestException("Doğrulama kodu geçersiz veya süresi dolmuş.");
            }

            return challenge;
        }

        private static void EnsureChallengeIsUsable(VerificationChallenge challenge, DateTime now)
        {
            if (challenge.ExpiresAt <= now)
            {
                throw new BadRequestException("Doğrulama kodu geçersiz veya süresi dolmuş.");
            }

            if (challenge.AttemptCount >= challenge.MaxAttempts)
            {
                throw new TooManyRequestsException("Doğrulama kodu için maksimum deneme hakkı aşıldı.");
            }
        }

        private bool IsCodeMatch(VerificationChallenge challenge, Guid userId, VerificationPurpose purpose, string? targetEmail, string code)
        {
            var expectedHash = HashCode(userId, purpose, targetEmail, code);
            return CryptographicOperations.FixedTimeEquals(Convert.FromHexString(challenge.CodeHash), Convert.FromHexString(expectedHash));
        }

        private static void MarkFailedAttempt(VerificationChallenge challenge, bool codeMatches, DateTime now)
        {
            if (codeMatches)
            {
                return;
            }

            challenge.AttemptCount++;
            challenge.UpdateAt = now;
        }

        private int GetExpirationMinutes()
        {
            return int.TryParse(_configuration["VerificationCode:ExpirationMinutes"], out var minutes) && minutes > 0 ? minutes : 10;
        }

        private int GetMaxAttempts()
        {
            return int.TryParse(_configuration["VerificationCode:MaxAttempts"], out var attempts) && attempts > 0 ? attempts : 5;
        }

        private static string? NormalizeEmail(string? email)
        {
            return string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToLowerInvariant();
        }

    }
}
