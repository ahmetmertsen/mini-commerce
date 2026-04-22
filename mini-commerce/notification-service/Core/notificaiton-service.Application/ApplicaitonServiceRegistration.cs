using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using notification_service.Application.Features.Notification.Commands.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace notification_service.Application
{
    public static class ApplicaitonServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<CreateNotificationCommand>();

            });

            services.AddValidatorsFromAssemblyContaining<CreateNotificationCommandValidator>();

            return services;
        }
    }
}
