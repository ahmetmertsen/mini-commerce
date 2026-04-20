using FluentValidation;
using invoice_service.Application.Features.Invoice.Commands.Create;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoice_service.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<CreateInvoiceCommand>();
            });

            services.AddValidatorsFromAssemblyContaining<CreateInvoiceCommandValidator>();

            return services;
        }
    }
}
