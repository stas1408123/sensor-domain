using MassTransit;
using Shared.Events;

namespace SensorProcessor.Consumers
{
    public class EnergyUpdatedConsumer : IConsumer<EnergyUpdated>
    {
        public Task Consume(ConsumeContext<EnergyUpdated> context)
        {
            Console.WriteLine(context.Message);
            return Task.CompletedTask;
        }
    }
}
