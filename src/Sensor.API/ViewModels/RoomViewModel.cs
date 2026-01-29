namespace Sensor.API.ViewModels
{
    public class RoomViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<AirQualityViewModel> airQualities = new List<AirQualityViewModel>();

        public List<EnergyViewModel> energies = new List<EnergyViewModel>();

        public List<MotionViewModel> motions = new List<MotionViewModel>();
    }
}
