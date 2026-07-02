using auth_service.Domain.Enums;

namespace auth_service.Domain.Entities
{
    public class AuthOutbox
    {
        public Guid IdempotentToken { get; set; }
        public Guid CorrelationId { get; set; }
        public DateTime OccuredOn { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public bool IsSensitive { get; set; }
        public AuthOutboxStatus Status { get; set; } = AuthOutboxStatus.Pending;
        public int RetryCount { get; set; }
        public int MaxRetryCount { get; set; } = 5;
        public DateTime? NextAttemptAt { get; set; }
        public DateTime? LockedUntil { get; set; }
        public string? LockedBy { get; set; }
        public string? LastError { get; set; }
    }
}
