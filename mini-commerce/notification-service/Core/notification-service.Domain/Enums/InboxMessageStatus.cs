namespace notification_service.Domain.Enums
{
    public enum InboxMessageStatus
    {
        Received = 1,
        Processing = 2,
        Processed = 3,
        Failed = 4,
        Ignored = 5
    }
}
