using Sensor.Ingestor.Models;

namespace Sensor.Ingestor.HttpClient.Abstarction
{
    interface IWeakAPI
    {
        Task<List<SensorData>> GetDataAsync();
    }
}
