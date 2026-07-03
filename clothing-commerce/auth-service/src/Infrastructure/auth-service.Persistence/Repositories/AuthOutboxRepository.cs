using auth_service.Application.Repositories;
using auth_service.Domain.Entities;
using auth_service.Domain.Enums;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace auth_service.Persistence.Repositories
{
    public class AuthOutboxRepository : IAuthOutboxRepository
    {
        private readonly string _connectionString;

        public AuthOutboxRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection connection string is required.");
        }

        public async Task<IReadOnlyList<AuthOutbox>> ClaimPendingAsync(int batchSize, TimeSpan lockDuration, string lockedBy, CancellationToken cancellationToken)
        {
            const string sql = """
                ;WITH PendingMessages AS
                (
                    SELECT TOP (@BatchSize) *
                    FROM AuthOutboxes WITH (UPDLOCK, READPAST, ROWLOCK)
                    WHERE ProcessedDate IS NULL
                      AND RetryCount < MaxRetryCount
                      AND (NextAttemptAt IS NULL OR NextAttemptAt <= @Now)
                      AND
                      (
                          Status = @PendingStatus
                          OR (Status = @ProcessingStatus AND LockedUntil < @Now)
                      )
                    ORDER BY OccuredOn ASC
                )
                UPDATE PendingMessages
                    SET Status = @ProcessingStatus,
                        LockedUntil = @LockedUntil,
                        LockedBy = @LockedBy,
                        LastError = NULL
                OUTPUT INSERTED.IdempotentToken,
                       INSERTED.CorrelationId,
                       INSERTED.OccuredOn,
                       INSERTED.ProcessedDate,
                       INSERTED.Type,
                       INSERTED.Payload,
                       INSERTED.IsSensitive,
                       CASE INSERTED.Status
                           WHEN 'Pending' THEN 1
                           WHEN 'Processing' THEN 2
                           WHEN 'Processed' THEN 3
                           WHEN 'Failed' THEN 4
                       END AS Status,
                       INSERTED.RetryCount,
                       INSERTED.MaxRetryCount,
                       INSERTED.NextAttemptAt,
                       INSERTED.LockedUntil,
                       INSERTED.LockedBy,
                       INSERTED.LastError;
                """;

            var now = DateTime.UtcNow;
            await using var connection = new SqlConnection(_connectionString);
            var messages = await connection.QueryAsync<AuthOutbox>(new CommandDefinition(
                sql,
                new
                {
                    BatchSize = batchSize,
                    Now = now,
                    LockedUntil = now.Add(lockDuration),
                    LockedBy = lockedBy,
                    PendingStatus = AuthOutboxStatus.Pending.ToString(),
                    ProcessingStatus = AuthOutboxStatus.Processing.ToString()
                },
                cancellationToken: cancellationToken));

            return messages.AsList();
        }

        public async Task MarkProcessedAsync(Guid idempotentToken, DateTime processedAt, CancellationToken cancellationToken)
        {
            const string sql = """
                UPDATE AuthOutboxes
                   SET Status = @ProcessedStatus,
                       ProcessedDate = @ProcessedAt,
                       LockedUntil = NULL,
                       LockedBy = NULL,
                       NextAttemptAt = NULL,
                       LastError = NULL
                 WHERE IdempotentToken = @IdempotentToken;
                """;

            await using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(new CommandDefinition(
                sql,
                new
                {
                    IdempotentToken = idempotentToken,
                    ProcessedAt = processedAt,
                    ProcessedStatus = AuthOutboxStatus.Processed.ToString()
                },
                cancellationToken: cancellationToken));
        }

        public async Task MarkFailedAsync(Guid idempotentToken, string error, DateTime? nextAttemptAt, CancellationToken cancellationToken)
        {
            const string sql = """
                UPDATE AuthOutboxes
                   SET RetryCount = RetryCount + 1,
                       Status = CASE
                           WHEN RetryCount + 1 >= MaxRetryCount THEN @FailedStatus
                           ELSE @PendingStatus
                       END,
                       NextAttemptAt = CASE
                           WHEN RetryCount + 1 >= MaxRetryCount THEN NULL
                           ELSE @NextAttemptAt
                       END,
                       LockedUntil = NULL,
                       LockedBy = NULL,
                       LastError = @Error
                 WHERE IdempotentToken = @IdempotentToken;
                """;

            await using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(new CommandDefinition(
                sql,
                new
                {
                    IdempotentToken = idempotentToken,
                    Error = Truncate(error, 2048),
                    NextAttemptAt = nextAttemptAt,
                    PendingStatus = AuthOutboxStatus.Pending.ToString(),
                    FailedStatus = AuthOutboxStatus.Failed.ToString()
                },
                cancellationToken: cancellationToken));
        }

        public async Task<int> CleanupProcessedAsync(DateTime olderThan, CancellationToken cancellationToken)
        {
            const string sql = """
                DELETE FROM AuthOutboxes
                 WHERE Status = @ProcessedStatus
                   AND ProcessedDate IS NOT NULL
                   AND ProcessedDate < @OlderThan;
                """;

            await using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync(new CommandDefinition(
                sql,
                new
                {
                    OlderThan = olderThan,
                    ProcessedStatus = AuthOutboxStatus.Processed.ToString()
                },
                cancellationToken: cancellationToken));
        }

        public async Task<int> CleanupExpiredSensitiveAsync(DateTime olderThan, CancellationToken cancellationToken)
        {
            const string sql = """
                DELETE FROM AuthOutboxes
                 WHERE IsSensitive = 1
                   AND OccuredOn < @OlderThan
                   AND Status IN (@ProcessedStatus, @FailedStatus);
                """;

            await using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync(new CommandDefinition(
                sql,
                new
                {
                    OlderThan = olderThan,
                    ProcessedStatus = AuthOutboxStatus.Processed.ToString(),
                    FailedStatus = AuthOutboxStatus.Failed.ToString()
                },
                cancellationToken: cancellationToken));
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return value.Length <= maxLength ? value : value[..maxLength];
        }
    }
}
