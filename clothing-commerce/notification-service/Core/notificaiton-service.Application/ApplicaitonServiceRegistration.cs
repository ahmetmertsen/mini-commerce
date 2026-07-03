using Microsoft.Extensions.DependencyInjection;
using notification_service.Application.Abstractions.Services;
using notification_service.Application.Services;

namespace notification_service.Application
{
    public static class ApplicaitonServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining(typeof(ApplicaitonServiceRegistration));
            });

            services.AddScoped<INotificationTemplateRenderer, NotificationTemplateRenderer>();

            return services;
        }
    }
}
