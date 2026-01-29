using AutoMapper;
using Sensor.API.ViewModels;
using Sensor.BLL.Models;

namespace Sensor.API.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Room, RoomViewModel>().ReverseMap();
            CreateMap<AirQuality, AirQualityViewModel>().ReverseMap();
            CreateMap<Energy, EnergyViewModel>().ReverseMap();
            CreateMap<Motion, MotionViewModel>().ReverseMap();

        }
    }
}
