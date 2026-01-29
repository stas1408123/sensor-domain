namespace Sensor.BLL.Models
{
    public class AirQuality
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
        public int Co2 { get; set; }
        public int Pm25 { get; set; }
        public int Humidity { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
