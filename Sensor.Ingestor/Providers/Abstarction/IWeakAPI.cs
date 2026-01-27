using Sensor.Ingestor.Models;

namespace Sensor.Ingestor.Providers.Abstarction
{
    public interface IWeakAPI
    {
        public Task<List<SensorData>> GetDataAsync();
    }
}
