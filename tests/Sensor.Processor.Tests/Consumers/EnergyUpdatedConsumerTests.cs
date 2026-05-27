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

public sealed class EnergyUpdatedConsumerTests
{
    [Fact]
    public async Task Consume_when_room_exists_updates_and_publishes_energy_reason()
    {
        var roomId = Guid.NewGuid();
        var existing = new RoomEntity { Id = roomId, Name = "Office" };
        var ts = DateTime.UtcNow;
        var message = new EnergyUpdated { Name = "Office", Energy = 12.5, Timestamp = ts };

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

        var context = new Mock<ConsumeContext<EnergyUpdated>>();
        context.Setup(c => c.Message).Returns(message);

        var sut = new EnergyUpdatedConsumer(
            repository.Object,
            publisher.Object,
            NullLogger<EnergyUpdatedConsumer>.Instance);

        await sut.Consume(context.Object);

        existing.Energies.Should().ContainSingle();
        existing.Energies[0].ConsumptionEnergy.Should().Be(12.5);
        existing.Energies[0].Timestamp.Should().Be(ts);
        existing.Energies[0].Room.Should().BeSameAs(existing);

        repository.Verify(r => r.Update(existing), Times.Once);
        repository.Verify(r => r.Add(It.IsAny<RoomEntity>()), Times.Never);

        published!.Type.Should().Be(Reason.EnergyUpdated);
        published.RoomId.Should().Be(roomId);
    }

    [Fact]
    public async Task Consume_when_room_missing_adds_room_and_publishes()
    {
        var message = new EnergyUpdated { Name = "Hall", Energy = 3, Timestamp = DateTime.UtcNow };

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

        var context = new Mock<ConsumeContext<EnergyUpdated>>();
        context.Setup(c => c.Message).Returns(message);

        var sut = new EnergyUpdatedConsumer(
            repository.Object,
            publisher.Object,
            NullLogger<EnergyUpdatedConsumer>.Instance);

        await sut.Consume(context.Object);

        repository.Verify(
            r => r.Add(It.Is<RoomEntity>(e => e.Name == "Hall" && e.Energies.Count == 1)),
            Times.Once);
        publisher.Verify(
            p => p.Publish(It.Is<RoomUpdated>(n => n.Type == Reason.EnergyUpdated && n.RoomId != Guid.Empty)),
            Times.Once);
    }
}
