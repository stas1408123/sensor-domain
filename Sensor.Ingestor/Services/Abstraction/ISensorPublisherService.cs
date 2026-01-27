using Sensor.Ingestor.Models;

namespace Sensor.Ingestor.Services.Abstraction
{
    public interface ISensorPublisherService
    {
        public Task Publish(SensorData data);
    }
}
