using notification_service.Domain.Enums;
using Shared.Messages.Notification.Enums;

namespace notification_service.Domain.Entities
{
    public class NotificationInbox
    {
        public Guid Id { get; set; }
        public Guid MessageId { get; set; }
        public Guid CorrelationId { get; set; }
        public NotificationType Type { get; set; }
        public NotificationChannel Channel { get; set; }
        public string Payload { get; set; } = null!;
        public InboxMessageStatus Status { get; set; } = InboxMessageStatus.Received;
        public int RetryCount { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }
        public DateTime? FailedAt { get; set; }
        public string? Error { get; set; }

        public void MarkProcessing()
        {
            Status = InboxMessageStatus.Processing;
        }

        public void MarkProcessed(DateTime processedAt)
        {
            Status = InboxMessageStatus.Processed;
            ProcessedAt = processedAt;
            FailedAt = null;
            Error = null;
        }

        public void MarkFailed(string error, DateTime failedAt)
        {
            Status = InboxMessageStatus.Failed;
            FailedAt = failedAt;
            Error = error;
            RetryCount++;
        }
    }
}
