using Microsoft.EntityFrameworkCore;
using notification_service.Application.Dtos;
using notification_service.Application.Repositories;
using notification_service.Domain.Entities;
using notification_service.Persistence.Context;
using Shared.Messages.Notification.Enums;

namespace notification_service.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationServiceDbContext _context;

        public NotificationRepository(NotificationServiceDbContext context)
        {
            _context = context;
        }

        public Task<Notification?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Notifications.FirstOrDefaultAsync(notification => notification.Id == id, cancellationToken);
        }

        public Task<Notification?> GetByMessageIdAsync(Guid messageId, CancellationToken cancellationToken)
        {
            return _context.Notifications.FirstOrDefaultAsync(notification => notification.MessageId == messageId, cancellationToken);
        }

        public async Task<(List<NotificationDto> Items, int TotalCount)> GetPagedByUserIdAsync(Guid userId, int page, int size, bool onlyUnread, CancellationToken cancellationToken)
        {
            var safePage = Math.Max(1, page);
            var safeSize = Math.Clamp(size, 1, 100);
            var query = VisibleByUser(userId);

            if (onlyUnread)
            {
                query = query.Where(notification => !notification.IsRead);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(notification => notification.CreatedAt)
                .ThenByDescending(notification => notification.Id)
                .Skip((safePage - 1) * safeSize)
                .Take(safeSize)
                .Select(notification => new NotificationDto
                {
                    Id = notification.Id,
                    MessageId = notification.MessageId,
                    CorrelationId = notification.CorrelationId,
                    UserId = notification.UserId,
                    RecipientEmail = notification.RecipientEmail,
                    RecipientPhone = notification.RecipientPhone,
                    Type = notification.Type,
                    Channel = notification.Channel,
                    Subject = notification.Subject,
                    Body = notification.Body,
                    Status = notification.Status,
                    IsRead = notification.IsRead,
                    CreatedAt = notification.CreatedAt,
                    SentAt = notification.SentAt,
                    FailedAt = notification.FailedAt,
                    FailureReason = notification.FailureReason
                })
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken)
        {
            return VisibleByUser(userId).CountAsync(notification => !notification.IsRead, cancellationToken);
        }

        public async Task AddAsync(Notification notification, CancellationToken cancellationToken)
        {
            await _context.Notifications.AddAsync(notification, cancellationToken);
        }

        public async Task<bool> MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken)
        {
            var notification = await VisibleByUser(userId).FirstOrDefaultAsync(item => item.Id == notificationId, cancellationToken);
            if (notification == null)
            {
                return false;
            }

            if (!notification.IsRead)
            {
                var now = DateTime.UtcNow;
                notification.IsRead = true;
                notification.UpdatedAt = now;
            }

            return true;
        }

        public async Task<int> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken)
        {
            var notifications = await VisibleByUser(userId)
                .Where(notification => !notification.IsRead)
                .ToListAsync(cancellationToken);

            var now = DateTime.UtcNow;
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.UpdatedAt = now;
            }

            return notifications.Count;
        }

        public async Task<bool> SoftDeleteByIdAndUserAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken)
        {
            var notification = await VisibleByUser(userId).FirstOrDefaultAsync(item => item.Id == notificationId, cancellationToken);
            if (notification == null)
            {
                return false;
            }

            notification.isActive = false;
            notification.isDeleted = true;
            notification.UpdatedAt = DateTime.UtcNow;
            return true;
        }

        public Task<bool> ExistsRecentAsync(NotificationType type, Guid userId, DateTime createdAfter, CancellationToken cancellationToken)
        {
            return _context.Notifications
                .AsNoTracking()
                .AnyAsync(notification =>
                    notification.Type == type &&
                    notification.UserId == userId &&
                    notification.CreatedAt >= createdAfter &&
                    notification.isActive &&
                    !notification.isDeleted,
                    cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<Notification> VisibleByUser(Guid userId)
        {
            return _context.Notifications.Where(notification => notification.UserId == userId && notification.isActive && !notification.isDeleted);
        }

    }
}
