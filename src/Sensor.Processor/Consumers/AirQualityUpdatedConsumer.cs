using MassTransit;
using Shared.Events;

namespace SensorProcessor.Consumers
{
    public class AirQualityUpdatedConsumer : IConsumer<AirQualityUpdated>
    {
        public Task Consume(ConsumeContext<AirQualityUpdated> context)
        {
            Console.WriteLine(context.Message);
            return Task.CompletedTask;
        }
    }
}
