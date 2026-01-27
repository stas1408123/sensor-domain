using MassTransit;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Sensor.Ingestor.HttpClient;
using Sensor.Ingestor.Services;
using Sensor.Ingestor.Services.Abstraction;
using Sensor.Ingestor.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));


AddWeakAPIDependencies(builder);
AddBusDependepcies(builder);

builder.Services.AddScoped<IIngestorService, IngestorService>();
builder.Services.AddScoped<ISensorPublisherService, SensorPublisherService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

static void AddWeakAPIDependencies(WebApplicationBuilder builder)
{
    var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
    var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
        .RetryAsync(apiSettings.RetryCount);
    builder.Services.AddHttpClient("WeakApiClient", (serviceProvider, client) =>
    {
        var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
        client.BaseAddress = new Uri(apiSettings.BaseUrl);
        client.DefaultRequestHeaders.Add("X-Api-Key", apiSettings.ApiKey);
        client.Timeout = TimeSpan.FromSeconds(30);
    }).AddPolicyHandler(retryPolicy);
    builder.Services.AddScoped<WeakAPI>();
}

static void AddBusDependepcies(WebApplicationBuilder builder)
{
    var rabbitMqSettings = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqSettings>();
    builder.Services.AddMassTransit(x =>
    {
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
}