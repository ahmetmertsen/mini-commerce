using notification_service.Domain.Entities;

namespace notification_service.Application.Repositories
{
    public interface IMailTemplateRepository
    {
        Task<MailTemplate?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
