using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Sensor.Notification.Hubs;
using Shared.Events;

namespace Sensor.Notification.Consumers
{
    public class RoomUpdatedConsumer(IHubContext<RoomNotificationHub> hubContext) : IConsumer<RoomUpdated>
    {
        public async Task Consume(ConsumeContext<RoomUpdated> context)
        {
            var message = context.Message;

            await hubContext.Clients.All.SendAsync("RoomUpdate", message);
        }
    }
}
