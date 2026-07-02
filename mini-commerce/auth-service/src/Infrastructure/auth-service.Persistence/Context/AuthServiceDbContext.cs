using auth_service.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace auth_service.Persistence.Context
{
    public class AuthServiceDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AuthServiceDbContext(DbContextOptions<AuthServiceDbContext> options) : base(options) { }

        public DbSet<AuthSession> AuthSessions { get; set; }
        public DbSet<VerificationChallenge> VerificationChallenges { get; set; }
        public DbSet<AuthOutbox> AuthOutboxes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(user => user.Status)
                    .HasDefaultValue(Domain.Enums.UserStatus.Active);

                entity.Property(user => user.Status)
                    .HasConversion<string>()
                    .HasMaxLength(32)
                    .HasDefaultValue(Domain.Enums.UserStatus.Active);
            });

            modelBuilder.Entity<AuthSession>(entity =>
            {
                entity.HasKey(session => session.Id);

                entity.Property(session => session.RefreshTokenHash)
                    .HasMaxLength(64)
                    .IsRequired();

                entity.Property(session => session.DeviceName)
                    .HasMaxLength(100);

                entity.Property(session => session.UserAgent)
                    .HasMaxLength(500);

                entity.Property(session => session.IpAddress)
                    .HasMaxLength(64);

                entity.Property(session => session.RevokedReason)
                    .HasMaxLength(200);

                entity.HasOne(session => session.User)
                    .WithMany(user => user.AuthSessions)
                    .HasForeignKey(session => session.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(session => session.RefreshTokenHash)
                    .IsUnique();

                entity.HasIndex(session => new { session.UserId, session.RevokedAt });
                entity.HasIndex(session => session.TokenFamilyId);
                entity.HasIndex(session => session.ExpiresAt);
            });

            modelBuilder.Entity<VerificationChallenge>(entity =>
            {
                entity.Property(challenge => challenge.Purpose)
                    .HasConversion<string>()
                    .HasMaxLength(32);

                entity.Property(challenge => challenge.TargetEmail)
                    .HasMaxLength(256);

                entity.Property(challenge => challenge.CodeHash)
                    .HasMaxLength(64)
                    .IsRequired();

                entity.HasOne(challenge => challenge.User)
                    .WithMany(user => user.VerificationChallenges)
                    .HasForeignKey(challenge => challenge.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(challenge => new { challenge.UserId, challenge.Purpose, challenge.TargetEmail, challenge.ConsumedAt });
                entity.HasIndex(challenge => challenge.ExpiresAt);
            });

            modelBuilder.Entity<AuthOutbox>(entity =>
            {
                entity.HasKey(o => o.IdempotentToken);

                entity.Property(o => o.Type)
                    .HasMaxLength(256)
                    .IsRequired();

                entity.Property(o => o.Payload)
                    .IsRequired();

                entity.Property(o => o.Status)
                    .HasConversion<string>()
                    .HasMaxLength(32)
                    .HasDefaultValue(Domain.Enums.AuthOutboxStatus.Pending);

                entity.Property(o => o.MaxRetryCount)
                    .HasDefaultValue(5);

                entity.Property(o => o.LockedBy)
                    .HasMaxLength(128);

                entity.Property(o => o.LastError)
                    .HasMaxLength(2048);

                entity.HasIndex(o => new { o.Status, o.NextAttemptAt, o.OccuredOn });
                entity.HasIndex(o => o.LockedUntil);
                entity.HasIndex(o => o.CorrelationId);
                entity.HasIndex(o => o.ProcessedDate);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
