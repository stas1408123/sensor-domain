namespace Sensor.API.Tests.Infrastructure;

[CollectionDefinition(Name)]
public sealed class ApiIntegrationCollection : ICollectionFixture<ApiFixture>
{
    public const string Name = "ApiIntegration";
}
