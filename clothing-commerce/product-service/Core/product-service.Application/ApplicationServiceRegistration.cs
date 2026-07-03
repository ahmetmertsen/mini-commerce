using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using product_service.Application.Features.Product.Commands.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product_service.Application
{
    public static class ApplicaitonServiceRegistration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<CreateProductCommand>();
            });

            services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();

            return services;
        }
    }
}
