using MassTransit;
using Sensor.Notification.Consumers;
using Sensor.Notification.Hubs;
using Shared.Settings;

var builder = WebApplication.CreateBuilder(args);
var rabbitMqSettings = builder.Configuration.GetSection("RabbitMq").Get<RabbitMQSettings>();


builder.Services.AddSignalR();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<RoomUpdatedConsumer>();

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
builder.Services.AddMassTransitHostedService();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapHub<RoomNotificationHub>("/roomNotification");


app.Run();
