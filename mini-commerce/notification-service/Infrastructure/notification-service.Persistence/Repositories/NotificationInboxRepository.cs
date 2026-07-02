using Microsoft.EntityFrameworkCore;
using notification_service.Application.Repositories;
using notification_service.Domain.Entities;
using notification_service.Persistence.Context;

namespace notification_service.Persistence.Repositories
{
    public class NotificationInboxRepository : INotificationInboxRepository
    {
        private readonly NotificationServiceDbContext _context;

        public NotificationInboxRepository(NotificationServiceDbContext context)
        {
            _context = context;
        }

        public Task<NotificationInbox?> GetByMessageIdAsync(Guid messageId, CancellationToken cancellationToken)
        {
            return _context.NotificationInboxes.FirstOrDefaultAsync(inbox => inbox.MessageId == messageId, cancellationToken);
        }

        public async Task AddAsync(NotificationInbox inboxMessage, CancellationToken cancellationToken)
        {
            await _context.NotificationInboxes.AddAsync(inboxMessage, cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
