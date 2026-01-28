namespace Sensor.BLL.Models
{
    public class Energy
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public double ConsumptionEnergy { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
