using System.Text.Json.Serialization;

namespace Sensor.Ingestor.Models
{
    public class Payload
    {
        [JsonPropertyName("motionDetected")]
        public bool? MotionDetected { get; set; }


        [JsonPropertyName("co2")]
        public int? Co2 { get; set; }

        [JsonPropertyName("pm25")]
        public int? Pm25 { get; set; }

        [JsonPropertyName("humidity")]
        public int? Humidity { get; set; }


        [JsonPropertyName("energy")]
        public double? Energy { get; set; }
    }

}
