using MassTransit;
using Shared.Events;

namespace Sensor.Processor.Producers
{
    public class NotificationPublisher : INotificationPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public NotificationPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Publish(RoomUpdated data)
        {
            await _publishEndpoint.Publish(data);
        }
    }
}
