using MassTransit;
using Sensor.Ingestor.Models;
using Sensor.Ingestor.Services.Abstraction;
using Shared.Events;

namespace Sensor.Ingestor.Services
{
    public class SensorPublisherService : ISensorPublisherService
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public SensorPublisherService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Publish(SensorData data)
        {
            switch (data.Type)
            {
                case Models.Type.Motion:
                    var motionEvent = new MotionUpdated
                    {
                        Name = data.Name,
                        MotionDetected = data.Payload.MotionDetected ?? false,
                        Timestamp = DateTime.UtcNow
                    };
                    await _publishEndpoint.Publish(motionEvent);
                    break;
                case Models.Type.Energy:
                    var energyEvent = new EnergyUpdated
                    {
                        Name = data.Name,
                        Energy = data.Payload.Energy ?? 0,
                        Timestamp = DateTime.UtcNow
                    };
                    await _publishEndpoint.Publish(energyEvent);
                    break;
                case Models.Type.Air_quality:
                    var airQualityEvent = new AirQualityUpdated
                    {
                        Name = data.Name,
                        Co2 = data.Payload.Co2 ?? 0,
                        Pm25 = data.Payload.Pm25 ?? 0,
                        Humidity = data.Payload.Humidity ?? 0,
                        Timestamp = DateTime.UtcNow
                    };
                    await _publishEndpoint.Publish(airQualityEvent);
                    break;
                default:
                    break;
            }
        }
    }
}
