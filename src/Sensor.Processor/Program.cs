using MassTransit;
using SensorProcessor.Consumers;
using Shared.Settings;
using Sensor.DAL;

var builder = WebApplication.CreateBuilder(args);

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

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddDataAccess(builder.Configuration);
var app = builder.Build();

app.Run();
