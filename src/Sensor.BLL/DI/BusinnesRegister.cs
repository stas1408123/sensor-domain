using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sensor.BLL.Models;
using Sensor.BLL.Services;
using Sensor.BLL.Services.Abstaction;
using Sensor.DAL;
using Sensor.DAL.Entities;

namespace Sensor.BLL.DI
{
    public static class BusinnesRegister
    {
        public static void AddBusinessLogicDependency(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IGenericService<Room>, GenericService<Room, RoomEntity>>();

            services.AddDataAccess(config);
        }
    }
}
