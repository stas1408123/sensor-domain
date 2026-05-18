using Sensor.BLL.Models;

namespace Sensor.BLL.Services.Abstaction
{
    public interface IRoomService : IGenericService<Room>
    {
        Task<IReadOnlyList<Room>> GetPagedWithDateFilter(
            DateTime? from,
            DateTime? to,
            int page,
            int pageSize);
    }
}
