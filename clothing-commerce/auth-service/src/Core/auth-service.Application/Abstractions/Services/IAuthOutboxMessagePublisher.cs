using auth_service.Domain.Entities;

namespace auth_service.Application.Abstractions.Services
{
    public interface IAuthOutboxMessagePublisher
    {
        Task PublishAsync(AuthOutbox outboxMessage, CancellationToken cancellationToken);
    }
}
