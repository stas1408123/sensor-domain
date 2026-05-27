using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace Sensor.API.Tests.Infrastructure;

internal static class GraphQlTestClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public static Task<HttpResponseMessage> PostAsync(
        HttpClient client,
        string query,
        object? variables = null) =>
        client.PostAsJsonAsync("/graphql", new { query, variables });

    public static async Task<JsonElement> ReadDataAsync(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var document = await response.Content.ReadFromJsonAsync<JsonDocument>(JsonOptions);
        document.Should().NotBeNull();

        if (document!.RootElement.TryGetProperty("errors", out var errors))
        {
            throw new InvalidOperationException(
                $"GraphQL returned errors: {errors.GetRawText()}");
        }

        document.RootElement.GetProperty("data").Should().NotBeNull();
        return document.RootElement.GetProperty("data");
    }
}
