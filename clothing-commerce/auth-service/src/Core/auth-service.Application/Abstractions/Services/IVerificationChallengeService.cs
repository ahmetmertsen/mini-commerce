using auth_service.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auth_service.Application.Abstractions.Services
{
    public interface IVerificationChallengeService
    {
        Task<string> CreateCodeAsync(Guid userId, VerificationPurpose purpose, string? targetEmail, Guid correlationId, CancellationToken cancellationToken);
        Task ValidateCodeAsync(Guid userId, VerificationPurpose purpose, string? targetEmail, string code, CancellationToken cancellationToken);
        Task ValidateEmailChangeCodesAsync(Guid userId, string oldEmail, string newEmail, string oldEmailCode, string newEmailCode, CancellationToken cancellationToken);
    }
}
