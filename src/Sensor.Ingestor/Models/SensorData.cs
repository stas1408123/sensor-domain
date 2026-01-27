using System.Text.Json.Serialization;

namespace Sensor.Ingestor.Models
{
    public class SensorData
    {
        [JsonPropertyName("type")]
        public Type Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("payload")]
        public Payload Payload { get; set; }
    }
}
