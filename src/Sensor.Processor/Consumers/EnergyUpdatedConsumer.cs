using MassTransit;
using Sensor.DAL.Entities;
using Sensor.DAL.Repositories.Abstarction;
using Sensor.Processor.Producers;
using Shared.Events;

namespace SensorProcessor.Consumers
{
    public class EnergyUpdatedConsumer(
        IGenericRepository<RoomEntity> repository,
        INotificationPublisher notificationPublisher,
        ILogger<EnergyUpdatedConsumer> logger) : IConsumer<EnergyUpdated>
    {
        public async Task Consume(ConsumeContext<EnergyUpdated> context)
        {
            var message = context.Message;
            logger.LogInformation("Processing energy update for room {RoomName}", message.Name);

            var energy = new EnergyEntity();
            energy.ConsumptionEnergy = message.Energy;
            energy.Timestamp = message.Timestamp;

            var room = (await repository.Get(x => x.Name == message.Name)).FirstOrDefault();

            if (room != null)
            {
                energy.Room = room;
                room.Energies.Add(energy);
                await repository.Update(room);
                await PublishNotification(room);
                logger.LogInformation("Updated existing room {RoomName} with energy data", room.Name);
                return;
            }

            room = new RoomEntity();
            room.Name = message.Name;

            room.Energies.Add(energy);
            await repository.Add(room);
            await PublishNotification(room);
            logger.LogInformation("Created room {RoomName} from energy data", room.Name);
        }

        private async Task PublishNotification(RoomEntity room)
        {
            var notification = new RoomUpdated();
            notification.Type = Reason.EnergyUpdated;
            notification.RoomId = room.Id;
            await notificationPublisher.Publish(notification);
        }
    }
}
