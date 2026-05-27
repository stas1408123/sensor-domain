using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sensor.DAL;
using Sensor.DAL.Entities;

namespace Sensor.API.Tests.Infrastructure;

internal static class DatabaseSeeder
{
    public static async Task ClearRoomsAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SensorDbContext>();
        await db.Rooms.ExecuteDeleteAsync();
    }

    public static async Task<RoomEntity> SeedRoomAsync(
        IServiceProvider services,
        string name,
        Guid? id = null)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SensorDbContext>();

        var room = new RoomEntity
        {
            Id = id ?? Guid.NewGuid(),
            Name = name,
        };

        db.Rooms.Add(room);
        await db.SaveChangesAsync();
        return room;
    }
}
