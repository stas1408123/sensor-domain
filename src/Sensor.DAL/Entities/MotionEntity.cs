using System.ComponentModel.DataAnnotations;

namespace Sensor.DAL.Entities
{
    public class MotionEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public bool MotionDetected { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
