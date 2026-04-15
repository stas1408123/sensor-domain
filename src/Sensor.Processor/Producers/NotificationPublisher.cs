using MassTransit;
using Shared.Events;

namespace Sensor.Processor.Producers
{
    public class NotificationPublisher : INotificationPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<NotificationPublisher> _logger;

        public NotificationPublisher(IPublishEndpoint publishEndpoint, ILogger<NotificationPublisher> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Publish(RoomUpdated data)
        {
            await _publishEndpoint.Publish(data);
            _logger.LogInformation("Published room notification for room {RoomId} with reason {Reason}", data.RoomId, data.Type);
        }
    }
}
