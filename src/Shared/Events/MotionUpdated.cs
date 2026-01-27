namespace Shared.Events
{
    public record MotionUpdated
    {
        public string Name { get; set; }
        public bool MotionDetected { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
