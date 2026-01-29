using Sensor.API.ViewModels;

namespace Sensor.API.GraphQL.Types
{
    public class MotionType : ObjectType<MotionViewModel>
    {
        protected override void Configure(IObjectTypeDescriptor<MotionViewModel> descriptor)
        {
            descriptor.Field(m => m.Id).Type<NonNullType<IdType>>();
            descriptor.Field(m => m.RoomId).Type<NonNullType<IdType>>();
            descriptor.Field(m => m.MotionDetected).Type<NonNullType<BooleanType>>();
            descriptor.Field(m => m.Timestamp).Type<NonNullType<DateTimeType>>();
        }
    }
}
