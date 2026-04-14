using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using product_service.Application.Repositories;
using product_service.Persistence.Context;
using product_service.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product_service.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProductServiceDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                );

            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
