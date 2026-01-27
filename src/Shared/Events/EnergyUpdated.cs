namespace Shared.Events
{
    public record EnergyUpdated
    {
        public string Name { get; set; }
        public double Energy { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
