using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sensor.DAL.Entities
{
    public class MotionEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }

        [ForeignKey("RoomId")]
        public RoomEntity Room { get; set; }
        public bool MotionDetected { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
