using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Sensor.Notification.Consumers;
using Sensor.Notification.Hubs;
using Shared.Events;

namespace Sensor.Notification.Tests.Consumers;

public sealed class RoomUpdatedConsumerTests
{
    [Fact]
    public async Task Consume_sends_room_update_to_all_clients()
    {
        var roomId = Guid.NewGuid();
        var message = new RoomUpdated { RoomId = roomId, Type = Reason.MotionUpdated };

        var clientProxy = new RecordingClientProxy();

        var hubClients = new Mock<IHubClients>();
        hubClients.Setup(h => h.All).Returns(clientProxy);

        var hubContext = new Mock<IHubContext<RoomNotificationHub>>();
        hubContext.Setup(h => h.Clients).Returns(hubClients.Object);

        var context = new Mock<ConsumeContext<RoomUpdated>>();
        context.Setup(c => c.Message).Returns(message);

        var sut = new RoomUpdatedConsumer(hubContext.Object, NullLogger<RoomUpdatedConsumer>.Instance);

        await sut.Consume(context.Object);

        clientProxy.Invocations.Should().ContainSingle();
        clientProxy.Invocations[0].MethodName.Should().Be("RoomUpdate");

        var sentPayload = clientProxy.Invocations[0].Payload.Should().BeOfType<RoomUpdated>().Subject;
        sentPayload.RoomId.Should().Be(roomId);
        sentPayload.Type.Should().Be(Reason.MotionUpdated);
    }

    private sealed class RecordingClientProxy : IClientProxy
    {
        public List<(string MethodName, object? Payload)> Invocations { get; } = [];

        public Task SendCoreAsync(
            string methodName,
            object?[] args,
            CancellationToken cancellationToken = default)
        {
            Invocations.Add((methodName, args.Length > 0 ? args[0] : null));
            return Task.CompletedTask;
        }
    }
}
