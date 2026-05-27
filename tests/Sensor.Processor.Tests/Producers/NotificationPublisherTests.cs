using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Sensor.Processor.Producers;
using Shared.Events;

namespace Sensor.Processor.Tests.Producers;

public sealed class NotificationPublisherTests
{
    [Fact]
    public async Task Publish_forwards_payload_to_publish_endpoint()
    {
        var roomId = Guid.NewGuid();
        var payload = new RoomUpdated { RoomId = roomId, Type = Reason.AirQualityUpdated };

        var endpoint = new Mock<IPublishEndpoint>();
        endpoint
            .Setup(e => e.Publish(It.IsAny<RoomUpdated>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new NotificationPublisher(endpoint.Object, NullLogger<NotificationPublisher>.Instance);

        await sut.Publish(payload);

        endpoint.Verify(
            e => e.Publish(
                It.Is<RoomUpdated>(m => m.RoomId == roomId && m.Type == Reason.AirQualityUpdated),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
