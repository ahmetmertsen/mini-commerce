using cart_service.Application.Repositories;
using cart_service.Persistence.Context;
using cart_service.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cart_service.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CartServiceDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                );

            services.AddScoped<ICartRepository, CartRepository>();

            return services;
        }
    }
}
