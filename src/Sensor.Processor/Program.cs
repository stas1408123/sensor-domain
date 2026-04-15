using MassTransit;
using SensorProcessor.Consumers;
using Shared.Settings;
using Sensor.DAL;
using Sensor.Processor.Producers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

var rabbitMqSettings = builder.Configuration.GetSection("RabbitMq").Get<RabbitMQSettings>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AirQualityUpdatedConsumer>();
    x.AddConsumer<EnergyUpdatedConsumer>();
    x.AddConsumer<MotionUpdatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.VirtualHost, h =>
        {
            h.Username(rabbitMqSettings.Username);
            h.Password(rabbitMqSettings.Password);
        });
        cfg.UseMessageRetry(r =>
        {
            r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
        });

        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddMassTransitHostedService();
builder.Services.AddScoped<INotificationPublisher, NotificationPublisher>();

builder.Services.AddDataAccess(builder.Configuration);
var app = builder.Build();
app.Logger.LogInformation("Sensor processor is starting");

app.Run();
