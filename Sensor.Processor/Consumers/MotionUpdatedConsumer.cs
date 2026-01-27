using MassTransit;
using Shared.Events;

namespace SensorProcessor.Consumers
{
    public class MotionUpdatedConsumer : IConsumer<MotionUpdated>
    {
        public Task Consume(ConsumeContext<MotionUpdated> context)
        {

            Console.WriteLine(context.Message);
            return Task.CompletedTask;
        }
    }
}
