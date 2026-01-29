namespace Sensor.API.ViewModels
{
    public class EnergyViewModel
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public double ConsumptionEnergy { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
