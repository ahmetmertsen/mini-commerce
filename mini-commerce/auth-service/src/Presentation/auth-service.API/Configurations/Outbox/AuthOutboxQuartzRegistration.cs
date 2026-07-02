using auth_service.API.Jobs;
using Quartz;

namespace auth_service.API.Configurations.Outbox
{
    public static class AuthOutboxQuartzRegistration
    {
        public static IServiceCollection AddAuthOutboxQuartz(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection(AuthOutboxOptions.SectionName).Get<AuthOutboxOptions>() ?? new AuthOutboxOptions();
            services.Configure<AuthOutboxOptions>(configuration.GetSection(AuthOutboxOptions.SectionName));

            services.AddQuartz(quartz =>
            {
                var dispatcherJobKey = new JobKey(nameof(AuthOutboxDispatcherJob));
                quartz.AddJob<AuthOutboxDispatcherJob>(job => job.WithIdentity(dispatcherJobKey));
                quartz.AddTrigger(trigger => trigger
                    .ForJob(dispatcherJobKey)
                    .WithIdentity($"{nameof(AuthOutboxDispatcherJob)}-trigger")
                    .StartNow()
                    .WithSimpleSchedule(schedule => schedule
                        .WithIntervalInSeconds(Math.Max(1, options.DispatcherIntervalSeconds))
                        .RepeatForever()));

                var cleanupJobKey = new JobKey(nameof(AuthOutboxCleanupJob));
                quartz.AddJob<AuthOutboxCleanupJob>(job => job.WithIdentity(cleanupJobKey));
                quartz.AddTrigger(trigger => trigger
                    .ForJob(cleanupJobKey)
                    .WithIdentity($"{nameof(AuthOutboxCleanupJob)}-trigger")
                    .StartNow()
                    .WithSimpleSchedule(schedule => schedule
                        .WithIntervalInMinutes(Math.Max(1, options.CleanupIntervalMinutes))
                        .RepeatForever()));
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            return services;
        }
    }
}
