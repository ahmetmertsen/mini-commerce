using Microsoft.EntityFrameworkCore;
using notification_service.Application.Repositories;
using notification_service.Domain.Entities;
using notification_service.Persistence.Context;

namespace notification_service.Persistence.Repositories
{
    public class MailTemplateRepository : IMailTemplateRepository
    {
        private readonly NotificationServiceDbContext _context;

        public MailTemplateRepository(NotificationServiceDbContext context)
        {
            _context = context;
        }

        public Task<MailTemplate?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return _context.MailTemplates
                .AsNoTracking()
                .FirstOrDefaultAsync(template => template.Name == name && template.isActive && !template.isDeleted, cancellationToken);
        }
    }
}
