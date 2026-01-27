using Sensor.Ingestor.Models;
using Sensor.Ingestor.Providers.Abstarction;
using Sensor.Ingestor.Services.Abstraction;

namespace Sensor.Ingestor.Services
{
    public class IngestorService : IIngestorService
    {
        public readonly ISensorPublisherService sensorPublisherService;
        public readonly IWeakAPI weakAPI;

        public IngestorService(ISensorPublisherService sensorPublisherService, IWeakAPI weakAPI)
        {
            this.sensorPublisherService = sensorPublisherService;
            this.weakAPI = weakAPI;
        }

        public async Task Ingest()
        {
            var data = await weakAPI.GetDataAsync();

            foreach (SensorData sensorData in data)
            {
                await sensorPublisherService.Publish(sensorData);
            }
        }
    }
}
