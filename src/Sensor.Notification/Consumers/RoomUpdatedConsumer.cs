using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Sensor.Notification.Hubs;
using Shared.Events;

namespace Sensor.Notification.Consumers
{
    public class RoomUpdatedConsumer(
        IHubContext<RoomNotificationHub> hubContext,
        ILogger<RoomUpdatedConsumer> logger) : IConsumer<RoomUpdated>
    {
        public async Task Consume(ConsumeContext<RoomUpdated> context)
        {
            var message = context.Message;
            logger.LogInformation("Received room update notification for room {RoomId} with reason {Reason}", message.RoomId, message.Type);

            await hubContext.Clients.All.SendAsync("RoomUpdate", message);
            logger.LogInformation("Broadcasted room update notification for room {RoomId}", message.RoomId);
        }
    }
}
