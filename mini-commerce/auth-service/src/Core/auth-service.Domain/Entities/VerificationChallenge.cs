using auth_service.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auth_service.Domain.Entities
{
    public class VerificationChallenge
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public VerificationPurpose Purpose { get; set; }
        public string? TargetEmail { get; set; }
        public string CodeHash { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public int AttemptCount { get; set; }
        public int MaxAttempts { get; set; }
        public DateTime? ConsumedAt { get; set; }
        public DateTime LastSentAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateAt { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }

        public Guid CorrelationId { get; set; }


        public User User { get; set; } = null!;
    }
}
