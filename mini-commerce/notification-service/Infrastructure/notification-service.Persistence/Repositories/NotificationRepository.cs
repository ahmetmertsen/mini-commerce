using Microsoft.EntityFrameworkCore;
using notificaiton_service.Application.Repositories;
using notification_service.Domain.Entities;
using notification_service.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_service.Persistence.Repositories
{
    public class NotificationRepository : INotificatitonRepository
    {
        private readonly NotificationServiceDbContext _context;

        public NotificationRepository(NotificationServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Notification?> GetByIdAsync(Guid id) => await _context.Notifications
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(Notification notification) => await _context.Notifications.AddAsync(notification);

        public void Update(Notification notification) => _context.Notifications.Update(notification);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<List<Notification>> GetByCustomerIdAsync(Guid customerId) => await _context.Notifications
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
    }
}
