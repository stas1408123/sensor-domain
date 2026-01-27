using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sensor.DAL.Repositories;
using Sensor.DAL.Repositories.Abstarction;

namespace Sensor.DAL
{
    public static class DataAccessRegister
    {
        public static void AddDataAccess(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddDbContext<SensorDbContext>((options) =>
            {
                options.UseInMemoryDatabase("SensorDB");
            });
        }
    }
}
