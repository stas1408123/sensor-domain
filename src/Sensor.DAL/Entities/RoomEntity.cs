using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sensor.DAL.Entities
{
    public class RoomEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<AirQualityEntity> AirQualities { get; set; } = new List<AirQualityEntity>();

        public List<EnergyEntity> Energies { get; set; } = new List<EnergyEntity>();

        public List<MotionEntity> Motions { get; set; } = new List<MotionEntity>();
    }
}
