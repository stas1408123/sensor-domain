using Microsoft.EntityFrameworkCore;
using Sensor.DAL.Entities;

namespace Sensor.DAL
{
    public class SensorDbContext : DbContext
    {
        public SensorDbContext(DbContextOptions<SensorDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<AirQualityEntity> AirQualities { get; set; }
        public DbSet<EnergyEntity> Energy { get; set; }
        public DbSet<MotionEntity> Motions { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomEntity>(entity =>
            {
                entity.Property(r => r.Name).IsRequired();
                entity.HasIndex(r => r.Name).IsUnique();
            });

            modelBuilder.Entity<AirQualityEntity>(entity =>
            {
                entity.HasIndex(a => a.RoomId);
                entity.HasIndex(a => a.Timestamp).IsDescending();
            });

            modelBuilder.Entity<EnergyEntity>(entity =>
            {
                entity.HasIndex(e => e.RoomId);
                entity.HasIndex(e => e.Timestamp).IsDescending();
            });

            modelBuilder.Entity<MotionEntity>(entity =>
            {
                entity.HasIndex(m => m.RoomId);
                entity.HasIndex(m => m.Timestamp).IsDescending();
            });
        }
    }
}
