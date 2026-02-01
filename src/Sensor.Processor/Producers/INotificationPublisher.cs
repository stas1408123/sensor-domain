using Shared.Events;

namespace Sensor.Processor.Producers
{
    public interface INotificationPublisher
    {
        public Task Publish(RoomUpdated data);
    }
}
