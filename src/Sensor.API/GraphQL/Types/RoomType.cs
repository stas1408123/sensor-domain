using Sensor.API.ViewModels;


namespace Sensor.API.GraphQL.Types
{
    public class RoomType : ObjectType<RoomViewModel>
    {
        protected override void Configure(IObjectTypeDescriptor<RoomViewModel> descriptor)
        {
            descriptor.Field(r => r.Id).Type<NonNullType<IdType>>();
            descriptor.Field(r => r.Name).Type<NonNullType<StringType>>();

            descriptor.Field("airQualities")
                .Type<ListType<NonNullType<AirQualityType>>>()
                .Resolve(context =>
                {
                    var room = context.Parent<RoomViewModel>();
                    return room.airQualities;
                });

            descriptor.Field("energies")
                .Type<ListType<NonNullType<EnergyType>>>()
                .Resolve(context =>
                {
                    var room = context.Parent<RoomViewModel>();
                    return room.energies;
                });

            descriptor.Field("motions")
                .Type<ListType<NonNullType<MotionType>>>()
                .Resolve(context =>
                {
                    var room = context.Parent<RoomViewModel>();
                    return room.motions;
                });
        }
    }
}
