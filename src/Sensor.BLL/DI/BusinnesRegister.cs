using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sensor.BLL.Services;
using Sensor.BLL.Services.Abstaction;
using Sensor.DAL;

namespace Sensor.BLL.DI
{
    public static class BusinnesRegister
    {
        public static void AddBusinessLogicDependency(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<,>));

            services.AddDataAccess(config);
        }
    }
}
