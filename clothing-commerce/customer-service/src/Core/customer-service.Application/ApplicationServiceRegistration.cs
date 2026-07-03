using customer_service.Application.Common;
using customer_service.Application.Features.Customer.Commands.CreateGuestCustomer;
using customer_service.Application.Mapping;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace customer_service.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<CreateGuestCustomerCommand>();
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });


            services.AddAutoMapper(cfg => { }, typeof(CustomerProfile).Assembly);
            services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

            return services;
        }
    }
}
