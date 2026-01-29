namespace Sensor.API.ViewModels
{
    public class MotionViewModel
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public bool MotionDetected { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
