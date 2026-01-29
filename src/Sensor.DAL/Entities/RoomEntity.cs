using System.ComponentModel.DataAnnotations;

namespace Sensor.DAL.Entities
{
    public class RoomEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<AirQualityEntity> AirQualities = new List<AirQualityEntity>();

        public List<EnergyEntity> Energies = new List<EnergyEntity>();

        public List<MotionEntity> Motions = new List<MotionEntity>();
    }
}
