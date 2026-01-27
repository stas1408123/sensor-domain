namespace Shared.Events
{
    public record AirQualityUpdated
    {
        public string Name { get; set; }
        public int Co2 { get; set; }
        public int Pm25 { get; set; }
        public int Humidity { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
