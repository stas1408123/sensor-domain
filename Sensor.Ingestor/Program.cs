using Hangfire;
using Hangfire.MemoryStorage;
using MassTransit;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Sensor.Ingestor.Providers;
using Sensor.Ingestor.Providers.Abstarction;
using Sensor.Ingestor.Services;
using Sensor.Ingestor.Services.Abstraction;
using Sensor.Ingestor.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

AddHangFire(builder);
AddWeakAPIDependencies(builder);
AddBusDependepcies(builder);

builder.Services.AddScoped<IIngestorService, IngestorService>();
builder.Services.AddScoped<ISensorPublisherService, SensorPublisherService>();
builder.Services.AddScoped<IWeakAPI, WeakAPI>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard();
}

app.MapGet("/", () => "Hello World!");

RecurringJob.AddOrUpdate<IIngestorService>(
    "IngestJob",
    service => service.Ingest(),
    "*/5 * * * *");

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

static void AddHangFire(WebApplicationBuilder builder)
{
    var hangfireSection = builder.Configuration.GetSection("Hangfire");
    var useInMemory = hangfireSection.GetValue<bool>("InMemory");
    if (useInMemory)
    {
        builder.Services.AddHangfire(config => config.UseMemoryStorage());
    }
    builder.Services.AddHangfireServer();
}