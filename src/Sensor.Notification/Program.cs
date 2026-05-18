using MassTransit;
using Sensor.Notification.Consumers;
using Sensor.Notification.Hubs;
using Serilog;
using Shared.Settings;

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


builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

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

Log.Information("MassTransit configured for RabbitMQ host {Host}", rabbitMqSettings.Host);
var app = builder.Build();
app.Logger.LogInformation("Sensor notification service is starting");

app.UseCors();

app.MapGet("/", () => "Hello World!");

app.MapHub<RoomNotificationHub>("/roomNotification");
app.Logger.LogInformation("SignalR hub mapped at /roomNotification");


app.Run();
