namespace Sensor.BLL.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<AirQuality> airQualities = new List<AirQuality>();

        public List<Energy> energies = new List<Energy>();

        public List<Motion> motions = new List<Motion>();
    }
}
