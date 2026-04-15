using Sensor.Ingestor.Models;
using Sensor.Ingestor.Providers.Abstarction;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sensor.Ingestor.Providers
{
    public class WeakAPI : IWeakAPI
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<WeakAPI> _logger;

        public WeakAPI(IHttpClientFactory httpClientFactory, ILogger<WeakAPI> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<List<SensorData>> GetDataAsync()
        {
            return await GetAsync<List<SensorData>>("/meters");
        }

        private async Task<T> GetAsync<T>(string requestUri)
        {
            var client = _httpClientFactory.CreateClient("WeakApiClient");
            _logger.LogInformation("Sending request to weak API endpoint {RequestUri}", requestUri);
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            _logger.LogDebug("Received response payload from endpoint {RequestUri}", requestUri);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } };
            return JsonSerializer.Deserialize<T>(json, options) ?? throw new Exception("Deserialize failed");
        }
    }
}
