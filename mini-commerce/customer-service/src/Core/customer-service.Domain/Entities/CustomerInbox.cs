using customer_service.Domain.Enums;

namespace customer_service.Domain.Entities
{
    public class CustomerInbox
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid MessageId { get; set; }
        public Guid CorrelationId { get; set; }
        public CustomerInboxMessageType MessageType { get; set; }
        public string Payload { get; set; } = null!;
        public InboxMessageStatus Status { get; set; } = InboxMessageStatus.Received;
        public int RetryCount { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }
        public DateTime? FailedAt { get; set; }
        public string? Error { get; set; }
    }
}
