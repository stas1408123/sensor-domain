using Microsoft.EntityFrameworkCore;
using Sensor.DAL.Entities;

namespace Sensor.DAL
{
    public class SensorDbContext : DbContext
    {
        public SensorDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<AirQualityEntity> AirQualities { get; set; }
        public DbSet<EnergyEntity> Energy { get; set; }
        public DbSet<MotionEntity> Motions { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
    }
}
