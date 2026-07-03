using notification_service.Application.Dtos;
using notification_service.Domain.Entities;
using Shared.Messages.Notification.Enums;

namespace notification_service.Application.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Notification?> GetByMessageIdAsync(Guid messageId, CancellationToken cancellationToken);
        Task<(List<NotificationDto> Items, int TotalCount)> GetPagedByUserIdAsync(Guid userId, int page, int size, bool onlyUnread, CancellationToken cancellationToken);
        Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken);
        Task AddAsync(Notification notification, CancellationToken cancellationToken);
        Task<bool> MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken);
        Task<int> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> SoftDeleteByIdAndUserAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken);
        Task<bool> ExistsRecentAsync(NotificationType type, Guid userId, DateTime createdAfter, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
