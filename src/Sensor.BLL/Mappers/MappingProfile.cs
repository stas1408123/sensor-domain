using AutoMapper;
using Sensor.BLL.Models;
using Sensor.DAL.Entities;

namespace Sensor.BLL.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Room, RoomEntity>().ReverseMap();
            CreateMap<AirQuality, AirQualityEntity>().ReverseMap();
            CreateMap<Energy, EnergyEntity>().ReverseMap();
            CreateMap<Motion, MotionEntity>().ReverseMap();

        }
    }
}
