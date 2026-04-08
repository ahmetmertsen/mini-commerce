using cart_service.Application.Features.Cart.Commands.AddItemToCart;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cart_service.Application
{
    public static class ApplicaitonServiceRegistration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<AddItemToCartCommand>();
            });

            services.AddValidatorsFromAssemblyContaining<AddItemToCartCommandValidator>();

            return services;
        }
    }
}
