using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Sensor.DAL;

namespace Sensor.API.Tests.Infrastructure;

public sealed class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public ApiWebApplicationFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        Environment.SetEnvironmentVariable(
            "ConnectionStrings__PostgreSqlConnection",
            _connectionString);

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("ConnectionStrings:PostgreSqlConnection", _connectionString);

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:PostgreSqlConnection"] = _connectionString,
            });
        });

        builder.ConfigureServices(services => ReplaceDbContext(services, _connectionString));
    }

    private static void ReplaceDbContext(IServiceCollection services, string connectionString)
    {
        var descriptors = services
            .Where(d =>
                d.ServiceType == typeof(SensorDbContext)
                || d.ServiceType == typeof(DbContextOptions<SensorDbContext>)
                || (d.ServiceType.IsGenericType
                    && d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>)))
            .ToList();

        foreach (var descriptor in descriptors)
            services.Remove(descriptor);

        services.RemoveAll<SensorDbContext>();
        services.RemoveAll<DbContextOptions<SensorDbContext>>();

        services.AddDbContext<SensorDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}
