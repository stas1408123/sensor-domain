namespace Sensor.Ingestor.Settings
{
    public class ApiSettings
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public int RetryCount { get; set; }
        public CircuitBreakerSettings CircuitBreaker { get; set; }
    }
}
