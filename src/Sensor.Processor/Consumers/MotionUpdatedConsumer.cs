using MassTransit;
using Sensor.DAL.Entities;
using Sensor.DAL.Repositories.Abstarction;
using Shared.Events;

namespace SensorProcessor.Consumers
{
    public class MotionUpdatedConsumer(IGenericRepository<RoomEntity> repository) : IConsumer<MotionUpdated>
    {
        public async Task Consume(ConsumeContext<MotionUpdated> context)
        {

            var message = context.Message;

            MotionEntity motion = new MotionEntity();
            motion.MotionDetected = message.MotionDetected;
            motion.Timestamp = message.Timestamp;

            var room = (await repository.Get(x => x.Name == context.Message.Name)).FirstOrDefault();

            if (room != null)
            {
                room.Motions.Add(motion);
                await repository.Update(room);
                return;
            }

            room = new RoomEntity();
            room.Name = message.Name;

            room.Motions.Add(motion);
            await repository.Add(room);
        }
    }
}
