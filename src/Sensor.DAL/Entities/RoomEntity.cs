using System.ComponentModel.DataAnnotations;

namespace Sensor.DAL.Entities
{
    public class RoomEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }

        public RoomEntity room { get; set; }
        public Guid Name { get; set; }

        public List<AirQualityEntity> airQualities = new List<AirQualityEntity>();

        public List<EnergyEntity> energies = new List<EnergyEntity>();

        public List<MotionEntity> motions = new List<MotionEntity>();
    }
}
