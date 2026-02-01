using MassTransit;
using Sensor.DAL.Entities;
using Sensor.DAL.Repositories.Abstarction;
using Sensor.Processor.Producers;
using Shared.Events;

namespace SensorProcessor.Consumers
{
    public class AirQualityUpdatedConsumer(IGenericRepository<RoomEntity> repository, INotificationPublisher notificationPublisher) : IConsumer<AirQualityUpdated>
    {
        
        public async Task Consume(ConsumeContext<AirQualityUpdated> context)
        {
            var message = context.Message;

            AirQualityEntity airQuality = CreateAirQuality(message);

            var room = (await repository.Get(x => x.Name == context.Message.Name)).FirstOrDefault();

            if (room != null)
            {
                room.AirQualities.Add(airQuality);
                await repository.Update(room);
                await PublishNotification(room);
                return;
            }

            room = new RoomEntity();
            room.Name = message.Name;

            room.AirQualities.Add(airQuality);
            await repository.Add(room);
            await PublishNotification(room);
        }

        private async Task PublishNotification(RoomEntity room)
        {
            var notification = new RoomUpdated();
            notification.Type = Reason.AirQualityUpdated;
            notification.RoomId = room.Id;
            await notificationPublisher.Publish(notification);
        }

        private static AirQualityEntity CreateAirQuality(AirQualityUpdated message)
        {
            var air = new AirQualityEntity();
            air.Humidity = message.Humidity;
            air.Pm25 = message.Pm25;
            air.Co2 = message.Co2;
            air.Timestamp = message.Timestamp;
            return air;
        }
    }
}
