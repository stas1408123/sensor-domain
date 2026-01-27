using System.ComponentModel.DataAnnotations;

namespace Sensor.DAL.Entities
{
    public class EnergyEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public double ConsumptionEnergy { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
