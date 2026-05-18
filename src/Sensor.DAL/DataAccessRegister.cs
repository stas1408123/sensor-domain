using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sensor.DAL.Entities;
using Sensor.DAL.Repositories;
using Sensor.DAL.Repositories.Abstarction;

namespace Sensor.DAL
{
    public static class DataAccessRegister
    {
        public static void AddDataAccess(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IGenericRepository<RoomEntity>, RoomRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            var connectionString = config.GetConnectionString("PostgreSqlConnection");
            services.AddDbContext<SensorDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
        }
    }
}
