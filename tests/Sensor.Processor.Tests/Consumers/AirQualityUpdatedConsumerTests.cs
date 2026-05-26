using System.Linq.Expressions;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Sensor.DAL.Entities;
using Sensor.DAL.Repositories.Abstarction;
using Sensor.Processor.Producers;
using SensorProcessor.Consumers;
using Shared.Events;

namespace Sensor.Processor.Tests.Consumers;

public sealed class AirQualityUpdatedConsumerTests
{
    [Fact]
    public async Task Consume_when_room_exists_updates_room_and_publishes_air_quality_reason()
    {
        var roomId = Guid.NewGuid();
        var existing = new RoomEntity { Id = roomId, Name = "Lab" };
        var ts = new DateTime(2025, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        var message = new AirQualityUpdated
        {
            Name = "Lab",
            Co2 = 400,
            Pm25 = 12,
            Humidity = 45,
            Timestamp = ts,
        };

        var repository = new Mock<IGenericRepository<RoomEntity>>();
        repository
            .Setup(r => r.Get(It.IsAny<Expression<Func<RoomEntity, bool>>>()))
            .ReturnsAsync(new List<RoomEntity> { existing });
        repository.Setup(r => r.Update(It.IsAny<RoomEntity>())).ReturnsAsync((RoomEntity r) => r);

        RoomUpdated? published = null;
        var publisher = new Mock<INotificationPublisher>();
        publisher
            .Setup(p => p.Publish(It.IsAny<RoomUpdated>()))
            .Callback<RoomUpdated>(n => published = n)
            .Returns(Task.CompletedTask);

        var context = new Mock<ConsumeContext<AirQualityUpdated>>();
        context.Setup(c => c.Message).Returns(message);

        var sut = new AirQualityUpdatedConsumer(
            repository.Object,
            publisher.Object,
            NullLogger<AirQualityUpdatedConsumer>.Instance);

        await sut.Consume(context.Object);

        existing.AirQualities.Should().ContainSingle();
        existing.AirQualities[0].Co2.Should().Be(400);
        existing.AirQualities[0].Pm25.Should().Be(12);
        existing.AirQualities[0].Humidity.Should().Be(45);
        existing.AirQualities[0].Timestamp.Should().Be(ts);

        repository.Verify(r => r.Update(existing), Times.Once);
        repository.Verify(r => r.Add(It.IsAny<RoomEntity>()), Times.Never);

        published.Should().NotBeNull();
        published!.RoomId.Should().Be(roomId);
        published.Type.Should().Be(Reason.AirQualityUpdated);
    }

    [Fact]
    public async Task Consume_when_room_missing_adds_room_and_publishes()
    {
        var message = new AirQualityUpdated
        {
            Name = "NewRoom",
            Co2 = 500,
            Pm25 = 20,
            Humidity = 50,
            Timestamp = DateTime.UtcNow,
        };

        var repository = new Mock<IGenericRepository<RoomEntity>>();
        repository
            .Setup(r => r.Get(It.IsAny<Expression<Func<RoomEntity, bool>>>()))
            .ReturnsAsync(Array.Empty<RoomEntity>());
        repository
            .Setup(r => r.Add(It.IsAny<RoomEntity>()))
            .ReturnsAsync((RoomEntity r) =>
            {
                if (r.Id == Guid.Empty)
                    r.Id = Guid.NewGuid();
                return r;
            });

        RoomUpdated? published = null;
        var publisher = new Mock<INotificationPublisher>();
        publisher
            .Setup(p => p.Publish(It.IsAny<RoomUpdated>()))
            .Callback<RoomUpdated>(n => published = n)
            .Returns(Task.CompletedTask);

        var context = new Mock<ConsumeContext<AirQualityUpdated>>();
        context.Setup(c => c.Message).Returns(message);

        var sut = new AirQualityUpdatedConsumer(
            repository.Object,
            publisher.Object,
            NullLogger<AirQualityUpdatedConsumer>.Instance);

        await sut.Consume(context.Object);

        repository.Verify(
            r => r.Add(It.Is<RoomEntity>(e => e.Name == "NewRoom" && e.AirQualities.Count == 1)),
            Times.Once);
        repository.Verify(r => r.Update(It.IsAny<RoomEntity>()), Times.Never);

        published.Should().NotBeNull();
        published!.Type.Should().Be(Reason.AirQualityUpdated);
        published.RoomId.Should().NotBeEmpty();
    }
}
