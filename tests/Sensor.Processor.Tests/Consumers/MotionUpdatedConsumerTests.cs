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

public sealed class MotionUpdatedConsumerTests
{
    [Fact]
    public async Task Consume_when_room_exists_updates_and_publishes_motion_reason()
    {
        var roomId = Guid.NewGuid();
        var existing = new RoomEntity { Id = roomId, Name = "Hallway" };
        var ts = DateTime.UtcNow;
        var message = new MotionUpdated { Name = "Hallway", MotionDetected = true, Timestamp = ts };

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

        var context = new Mock<ConsumeContext<MotionUpdated>>();
        context.Setup(c => c.Message).Returns(message);

        var sut = new MotionUpdatedConsumer(
            repository.Object,
            publisher.Object,
            NullLogger<MotionUpdatedConsumer>.Instance);

        await sut.Consume(context.Object);

        existing.Motions.Should().ContainSingle();
        existing.Motions[0].MotionDetected.Should().BeTrue();
        existing.Motions[0].Timestamp.Should().Be(ts);

        repository.Verify(r => r.Update(existing), Times.Once);
        repository.Verify(r => r.Add(It.IsAny<RoomEntity>()), Times.Never);

        published!.Type.Should().Be(Reason.MotionUpdated);
        published.RoomId.Should().Be(roomId);
    }

    [Fact]
    public async Task Consume_when_room_missing_adds_room_and_publishes()
    {
        var message = new MotionUpdated
        {
            Name = "Stairwell",
            MotionDetected = false,
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

        var publisher = new Mock<INotificationPublisher>();
        publisher.Setup(p => p.Publish(It.IsAny<RoomUpdated>())).Returns(Task.CompletedTask);

        var context = new Mock<ConsumeContext<MotionUpdated>>();
        context.Setup(c => c.Message).Returns(message);

        var sut = new MotionUpdatedConsumer(
            repository.Object,
            publisher.Object,
            NullLogger<MotionUpdatedConsumer>.Instance);

        await sut.Consume(context.Object);

        repository.Verify(
            r => r.Add(It.Is<RoomEntity>(e => e.Name == "Stairwell" && e.Motions.Count == 1)),
            Times.Once);
        publisher.Verify(
            p => p.Publish(It.Is<RoomUpdated>(n => n.Type == Reason.MotionUpdated)),
            Times.Once);
    }
}
