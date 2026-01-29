using Sensor.API.ViewModels;

namespace Sensor.API.GraphQL.Types
{
    public class EnergyType : ObjectType<EnergyViewModel>
    {
        protected override void Configure(IObjectTypeDescriptor<EnergyViewModel> descriptor)
        {
            descriptor.Field(e => e.Id).Type<NonNullType<IdType>>();
            descriptor.Field(e => e.RoomId).Type<NonNullType<IdType>>();
            descriptor.Field(e => e.ConsumptionEnergy).Type<NonNullType<FloatType>>();
            descriptor.Field(e => e.Timestamp).Type<NonNullType<DateTimeType>>();
        }
    }
}
