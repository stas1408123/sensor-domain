using Sensor.Ingestor.Models;

namespace Sensor.Ingestor.Services.Abstraction
{
    interface ISensorPublisherService
    {
        Task Publish(SensorData data);
    }
}
