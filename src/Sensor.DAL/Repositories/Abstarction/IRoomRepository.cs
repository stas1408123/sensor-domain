using Sensor.DAL.Entities;

namespace Sensor.DAL.Repositories.Abstarction
{
    public interface IRoomRepository : IGenericRepository<RoomEntity>
    {
        Task<IReadOnlyList<RoomEntity>> GetPagedWithDateFilter(
            DateTime? from,
            DateTime? to,
            int page,
            int pageSize);
    }
}
