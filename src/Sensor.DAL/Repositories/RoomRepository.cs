using Microsoft.EntityFrameworkCore;
using Sensor.DAL.Entities;
using Sensor.DAL.Repositories.Abstarction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sensor.DAL.Repositories
{
    public class RoomRepository : GenericRepository<RoomEntity>, IGenericRepository<RoomEntity>
    {
        public RoomRepository(SensorDbContext context) : base(context)
        {
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
    }
}
