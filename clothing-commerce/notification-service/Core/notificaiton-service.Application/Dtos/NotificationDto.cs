using notification_service.Domain.Enums;
using Shared.Messages.Notification.Enums;

namespace notification_service.Application.Dtos
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public Guid MessageId { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid? UserId { get; set; }
        public string? RecipientEmail { get; set; }
        public string? RecipientPhone { get; set; }
        public NotificationType Type { get; set; }
        public NotificationChannel Channel { get; set; }
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public NotificationStatus Status { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? FailedAt { get; set; }
        public string? FailureReason { get; set; }
    }
}
