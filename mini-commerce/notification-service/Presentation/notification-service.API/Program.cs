using MassTransit;
using notification_service.API.Consumers;
using notification_service.Application;
using notification_service.Infrastructure;
using notification_service.Persistence;
using Shared.Settings;

namespace notification_service.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices();

            #region MassTransit
            builder.Services.AddMassTransit(configurator =>
            {
                configurator.AddConsumer<NotificationRequestedConsumer>();

                configurator.UsingRabbitMq((context, configure) =>
                {
                    configure.Host(builder.Configuration["RabbitMQ"]);
                    configure.ReceiveEndpoint(RabbitMQSettings.NotificationRequestedQueue, endpoint =>
                    {
                        endpoint.ConfigureConsumer<NotificationRequestedConsumer>(context);
                    });
                });
            });
            #endregion

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
