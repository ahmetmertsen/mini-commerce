using checkout_orchestrator_service.CheckoutDbContext;
using checkout_orchestrator_service.CheckoutStateInstances;
using checkout_orchestrator_service.CheckoutStateMachines;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Settings;

namespace checkout_orchestrator_service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddMassTransit(configurator =>
            {
                configurator.AddSagaStateMachine<CartStateMachine, CartStateInstance>()
                    .EntityFrameworkRepository(options =>
                    {
                        options.AddDbContext<DbContext, CheckoutSagaDbContext>((provider, _builder) =>
                        {
                            _builder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                        });
                        options.UseSqlServer();
                    });

                configurator.UsingRabbitMq((context, _configure) =>
                {
                    _configure.Host(builder.Configuration["RabbitMQ"]);

                    _configure.ReceiveEndpoint(RabbitMQSettings.CheckoutStateMachineQueue, e => e.ConfigureSaga<CartStateInstance>(context));
                });
            });

            var host = builder.Build();
            host.Run();
        }
    }
}