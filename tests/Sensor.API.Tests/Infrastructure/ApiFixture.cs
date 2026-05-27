using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace Sensor.API.Tests.Infrastructure;

public sealed class ApiFixture : IAsyncLifetime
{
    public const string CiConnectionEnvVar = "POSTGRES_TEST_CONNECTION";

    private PostgreSqlContainer? _postgres;

    public ApiWebApplicationFactory Factory { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        var connectionString = Environment.GetEnvironmentVariable(CiConnectionEnvVar);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            _postgres = new PostgreSqlBuilder("postgres:16-alpine").Build();
            await _postgres.StartAsync();
            connectionString = _postgres.GetConnectionString();
        }

        Factory = new ApiWebApplicationFactory(connectionString);
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();

        if (_postgres is not null)
            await _postgres.DisposeAsync();
    }
}
