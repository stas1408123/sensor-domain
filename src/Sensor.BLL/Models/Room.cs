namespace Sensor.BLL.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }

        public Room room { get; set; }
        public Guid Name { get; set; }

        public List<AirQuality> airQualities = new List<AirQuality>();

        public List<Energy> energies = new List<Energy>();

        public List<Motion> motions = new List<Motion>();
    }
}
