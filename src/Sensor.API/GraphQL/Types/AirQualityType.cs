using Sensor.API.ViewModels;

namespace Sensor.API.GraphQL.Types
{
    public class AirQualityType : ObjectType<AirQualityViewModel>
    {
        protected override void Configure(IObjectTypeDescriptor<AirQualityViewModel> descriptor)
        {
            descriptor.Field(a => a.Id).Type<NonNullType<IdType>>();
            descriptor.Field(a => a.RoomId).Type<NonNullType<IdType>>();
            descriptor.Field(a => a.Co2).Type<NonNullType<IntType>>();
            descriptor.Field(a => a.Pm25).Type<NonNullType<IntType>>();
            descriptor.Field(a => a.Humidity).Type<NonNullType<IntType>>();
            descriptor.Field(a => a.Timestamp).Type<NonNullType<DateTimeType>>();
        }
    }
}
