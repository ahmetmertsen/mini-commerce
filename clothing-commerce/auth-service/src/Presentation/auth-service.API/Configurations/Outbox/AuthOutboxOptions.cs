namespace auth_service.API.Configurations.Outbox
{
    public class AuthOutboxOptions
    {
        public const string SectionName = "AuthOutbox";

        public int BatchSize { get; set; } = 25;
        public int DispatcherIntervalSeconds { get; set; } = 10;
        public int CleanupIntervalMinutes { get; set; } = 15;
        public int LockSeconds { get; set; } = 60;
        public int RetryBaseDelaySeconds { get; set; } = 10;
        public int MaxRetryDelaySeconds { get; set; } = 300;
        public int ProcessedRetentionMinutes { get; set; } = 60;
        public int SensitiveRetentionMinutes { get; set; } = 30;
    }
}
