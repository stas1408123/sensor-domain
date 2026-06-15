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

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

AddHangFire(builder);
AddWeakAPIDependencies(builder);
AddBusDependepcies(builder);

builder.Services.AddScoped<IIngestorService, IngestorService>();
builder.Services.AddScoped<ISensorPublisherService, SensorPublisherService>();
builder.Services.AddScoped<IWeakAPI, WeakAPI>();

var app = builder.Build();
app.Logger.LogInformation("Sensor ingestor is starting");

if (app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard();
}

app.MapGet("/", () => "Hello World!");

RecurringJob.AddOrUpdate<IIngestorService>(
    "IngestJob",
    service => service.Ingest(),
    "*/1 * * * *");
app.Logger.LogInformation("Hangfire recurring job IngestJob configured to run every minute");

app.Run();

static void AddWeakAPIDependencies(WebApplicationBuilder builder)
{
    var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
    var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
        .RetryAsync(apiSettings.RetryCount);
    var circuitBreakerPolicy = HttpPolicyExtensions.HandleTransientHttpError()
        .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: apiSettings.CircuitBreaker.FailureThreshold, durationOfBreak: TimeSpan.FromSeconds(apiSettings.CircuitBreaker.DurationOfBreakSeconds));
    var combinedPolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
    
    builder.Services.AddHttpClient("WeakApiClient", (serviceProvider, client) =>
    {
        var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
        client.BaseAddress = new Uri(apiSettings.BaseUrl);
        client.DefaultRequestHeaders.Add("X-Api-Key", apiSettings.ApiKey);
        client.Timeout = TimeSpan.FromSeconds(30);
    }).AddPolicyHandler(combinedPolicy);
    builder.Services.AddScoped<WeakAPI>();
    Log.Information("Weak API client configured with retry count {RetryCount} and circuit breaker ({FailureThreshold} failures, {DurationOfBreakSeconds}s break)", apiSettings.RetryCount, apiSettings.CircuitBreaker.FailureThreshold, apiSettings.CircuitBreaker.DurationOfBreakSeconds);
}

static void AddBusDependepcies(WebApplicationBuilder builder)
{
    var rabbitMqSettings = builder.Configuration.GetSection("RabbitMq").Get<RabbitMQSettings>();
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
    builder.Services.AddMassTransitHostedService();
    Log.Information("MassTransit configured for RabbitMQ host {Host}", rabbitMqSettings.Host);
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