namespace Shared.Events
{
    public class RoomUpdated
    {
        public Reason Type { get; set; }

        public Guid RoomId { get; set; }

    }

    public enum Reason
    {
        AirQualityUpdated,
        EnergyUpdated,
        MotionUpdated
    }
}
