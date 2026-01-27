using Sensor.Ingestor.HttpClient;
using Sensor.Ingestor.Models;
using Sensor.Ingestor.Services.Abstraction;

namespace Sensor.Ingestor.Services
{
    public class IngestorService : IIngestorService
    {
        public readonly SensorPublisherService sensorPublisherService;
        public readonly WeakAPI weakAPI;

        public IngestorService(SensorPublisherService sensorPublisherService, WeakAPI weakAPI)
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
            return data;
        }
    }
}
