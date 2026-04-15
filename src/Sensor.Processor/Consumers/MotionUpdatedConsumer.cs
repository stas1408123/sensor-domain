using MassTransit;
using Sensor.DAL.Entities;
using Sensor.DAL.Repositories.Abstarction;
using Sensor.Processor.Producers;
using Shared.Events;

namespace SensorProcessor.Consumers
{
    public class MotionUpdatedConsumer(
        IGenericRepository<RoomEntity> repository,
        INotificationPublisher notificationPublisher,
        ILogger<MotionUpdatedConsumer> logger) : IConsumer<MotionUpdated>
    {
        public async Task Consume(ConsumeContext<MotionUpdated> context)
        {

            var message = context.Message;
            logger.LogInformation("Processing motion update for room {RoomName}", message.Name);

            var motion = new MotionEntity();
            motion.MotionDetected = message.MotionDetected;
            motion.Timestamp = message.Timestamp;

            var room = (await repository.Get(x => x.Name == context.Message.Name)).FirstOrDefault();

            if (room != null)
            {
                room.Motions.Add(motion);
                await repository.Update(room);
                await PublishNotification(room);
                logger.LogInformation("Updated existing room {RoomName} with motion data", room.Name);
                return;
            }

            room = new RoomEntity();
            room.Name = message.Name;

            room.Motions.Add(motion);
            await repository.Add(room);
            await PublishNotification(room);
            logger.LogInformation("Created room {RoomName} from motion data", room.Name);
        }

        private async Task PublishNotification(RoomEntity room)
        {
            var notification = new RoomUpdated();
            notification.Type = Reason.MotionUpdated;
            notification.RoomId = room.Id;
            await notificationPublisher.Publish(notification);
        }
    }
}
