namespace Sensor.BLL.Models
{
    public class Motion
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public bool MotionDetected { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
