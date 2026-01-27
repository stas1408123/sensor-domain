namespace Sensor.Ingestor.Settings
{
    public class CircuitBreakerSettings
    {
        public int FailureThreshold { get; set; }
        public int DurationOfBreakSeconds { get; set; }
    }
}
