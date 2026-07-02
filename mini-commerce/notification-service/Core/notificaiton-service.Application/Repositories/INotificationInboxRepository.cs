using notification_service.Domain.Entities;

namespace notification_service.Application.Repositories
{
    public interface INotificationInboxRepository
    {
        Task<NotificationInbox?> GetByMessageIdAsync(Guid messageId, CancellationToken cancellationToken);
        Task AddAsync(NotificationInbox inboxMessage, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
