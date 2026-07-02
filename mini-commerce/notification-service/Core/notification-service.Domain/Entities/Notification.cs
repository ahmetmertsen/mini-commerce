using notification_service.Domain.Enums;
using Shared.Messages.Notification.Enums;

namespace notification_service.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid MessageId { get; set; }
        public Guid CorrelationId { get; set; }
        public NotificationType Type { get; set; }
        public NotificationChannel Channel { get; set; }
        public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
        public Guid? UserId { get; set; }
        public string? RecipientEmail { get; set; }
        public string? RecipientPhone { get; set; }
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public bool IsSensitive { get; set; }
        public bool isActive { get; set; } = true;
        public bool isDeleted { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? FailedAt { get; set; }
        public string? FailureReason { get; set; }

        public void MarkSent(DateTime sentAt)
        {
            Status = NotificationStatus.Sent;
            SentAt = sentAt;
            FailedAt = null;
            FailureReason = null;
            UpdatedAt = sentAt;
        }

        public void MarkFailed(string reason, DateTime failedAt)
        {
            Status = NotificationStatus.Failed;
            FailedAt = failedAt;
            FailureReason = reason;
            UpdatedAt = failedAt;
        }
    }
}
