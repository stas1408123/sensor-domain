using Sensor.Ingestor.Models;
using Sensor.Ingestor.Providers.Abstarction;
using Sensor.Ingestor.Services.Abstraction;

namespace Sensor.Ingestor.Services
{
    public class IngestorService : IIngestorService
    {
        public readonly ISensorPublisherService sensorPublisherService;
        public readonly IWeakAPI weakAPI;
        private readonly ILogger<IngestorService> _logger;

        public IngestorService(
            ISensorPublisherService sensorPublisherService,
            IWeakAPI weakAPI,
            ILogger<IngestorService> logger)
        {
            this.sensorPublisherService = sensorPublisherService;
            this.weakAPI = weakAPI;
            _logger = logger;
        }

        public async Task Ingest()
        {
            _logger.LogInformation("Starting ingest cycle");
            var data = await weakAPI.GetDataAsync();
            _logger.LogInformation("Fetched {Count} sensor records from weak API", data.Count);

            foreach (SensorData sensorData in data)
            {
                await sensorPublisherService.Publish(sensorData);
            }
            _logger.LogInformation("Finished ingest cycle");
        }
    }
}
