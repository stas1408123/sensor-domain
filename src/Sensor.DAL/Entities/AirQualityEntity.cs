using System.ComponentModel.DataAnnotations;

namespace Sensor.DAL.Entities
{
    public class AirQualityEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }
        public int Co2 { get; set; }
        public int Pm25 { get; set; }
        public int Humidity { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
