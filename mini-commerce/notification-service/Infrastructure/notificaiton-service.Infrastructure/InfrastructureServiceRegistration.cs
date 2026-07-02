using Microsoft.Extensions.DependencyInjection;
using notification_service.Application.Abstractions.Services;
using notification_service.Infrastructure.Services.Mail;

namespace notification_service.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IMailService, MailService>();

            return services;
        }
    }
}
