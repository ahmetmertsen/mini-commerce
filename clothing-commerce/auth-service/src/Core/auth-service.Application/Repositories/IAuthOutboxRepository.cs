using auth_service.Domain.Entities;

namespace auth_service.Application.Repositories
{
    public interface IAuthOutboxRepository
    {
        Task<IReadOnlyList<AuthOutbox>> ClaimPendingAsync(int batchSize, TimeSpan lockDuration, string lockedBy, CancellationToken cancellationToken);
        Task MarkProcessedAsync(Guid idempotentToken, DateTime processedAt, CancellationToken cancellationToken);
        Task MarkFailedAsync(Guid idempotentToken, string error, DateTime? nextAttemptAt, CancellationToken cancellationToken);
        Task<int> CleanupProcessedAsync(DateTime olderThan, CancellationToken cancellationToken);
        Task<int> CleanupExpiredSensitiveAsync(DateTime olderThan, CancellationToken cancellationToken);
    }
}
