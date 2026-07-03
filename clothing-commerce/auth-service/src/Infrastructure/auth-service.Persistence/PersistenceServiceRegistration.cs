using auth_service.Application.Abstractions.Services;
using auth_service.Application.Repositories;
using auth_service.Domain.Entities;
using auth_service.Persistence.Context;
using auth_service.Persistence.Repositories;
using auth_service.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace auth_service.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthServiceDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, Role>(options =>
            {
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AuthServiceDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthSessionService, AuthSessionService>();
            services.AddScoped<IVerificationChallengeService, VerificationChallengeService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAuthOutboxRepository, AuthOutboxRepository>();

            return services;
        }
    }
}
