using Microsoft.EntityFrameworkCore;
using Sensor.DAL.Entities;
using Sensor.DAL.Repositories.Abstarction;

namespace Sensor.DAL.Repositories
{
    public class RoomRepository : GenericRepository<RoomEntity>, IRoomRepository
    {
        public RoomRepository(SensorDbContext context) : base(context)
        {
        }

        public override async Task<RoomEntity> GetById(Guid id)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(x => x.AirQualities)
                .Include(x => x.Energies)
                .Include(x => x.Motions)
                .FirstAsync(x => x.Id == id);
        }

        public override async Task<IReadOnlyList<RoomEntity>> GetAll()
        {
            return await _dbSet
                .Include(x => x.AirQualities)
                .Include(x => x.Energies)
                .Include(x=> x.Motions)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<RoomEntity>> GetPagedWithDateFilter(
            DateTime? from,
            DateTime? to,
            int page,
            int pageSize)
        {
            var rangeStart = from ?? DateTime.MinValue;
            var rangeEnd = to ?? DateTime.MaxValue;
            var query = _dbSet.AsNoTracking();

            if (from.HasValue || to.HasValue)
            {
                query = query.Where(room =>
                    room.AirQualities.Any(a => a.Timestamp >= rangeStart && a.Timestamp <= rangeEnd) ||
                    room.Energies.Any(e => e.Timestamp >= rangeStart && e.Timestamp <= rangeEnd) ||
                    room.Motions.Any(m => m.Timestamp >= rangeStart && m.Timestamp <= rangeEnd));
            }

            return await query
                .OrderBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(r => r.AirQualities.Where(a => a.Timestamp >= rangeStart && a.Timestamp <= rangeEnd))
                .Include(r => r.Energies.Where(e => e.Timestamp >= rangeStart && e.Timestamp <= rangeEnd))
                .Include(r => r.Motions.Where(m => m.Timestamp >= rangeStart && m.Timestamp <= rangeEnd))
                .ToListAsync();
        }
    }
}
