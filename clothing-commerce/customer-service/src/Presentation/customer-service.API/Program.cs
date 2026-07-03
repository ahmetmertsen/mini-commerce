using customer_service.API.Consumers;
using customer_service.API.Middlewares;
using customer_service.Application;
using customer_service.Persistence;
using MassTransit;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Shared.Events.AuthEvents;
using Shared.Settings;

namespace customer_service.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Serilog
            var seqServerUrl = builder.Configuration["Seq:ServerURL"] ?? throw new InvalidOperationException("Seq:ServerURL configuration is required.");

            Logger log = new LoggerConfiguration()
                .WriteTo.File(
                    path: "logs/customerservice-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    fileSizeLimitBytes: 10_000_000,
                    rollOnFileSizeLimit: true,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                .WriteTo.File(
                    path: "logs/errors/error-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 60,
                    restrictedToMinimumLevel: LogEventLevel.Error
                )
                .WriteTo.Seq(seqServerUrl)
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .CreateLogger();

            builder.Host.UseSerilog(log);
            #endregion

            builder.Services.AddPersistenceService(builder.Configuration);
            builder.Services.AddApplicationService();

            #region MassTransit
            builder.Services.AddMassTransit(configurator =>
            {
                configurator.AddConsumer<AuthUserRegisteredEventConsumer>();
                configurator.AddConsumer<AuthUserEmailChangedEventConsumer>();

                configurator.UsingRabbitMq((context, configure) =>
                {
                    configure.Host(builder.Configuration["RabbitMQ"]);
                    configure.ReceiveEndpoint(RabbitMQSettings.AuthCustomerEventsQueue, endpoint =>
                    {
                        endpoint.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(5)));
                        endpoint.ConfigureConsumer<AuthUserRegisteredEventConsumer>(context);
                        endpoint.ConfigureConsumer<AuthUserEmailChangedEventConsumer>(context);
                    });
                });
            });
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
