namespace MediBook.AvailabilityService.DTOs
{
    public class AddSlotRequest
    {
        public string ProviderId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int DurationMinutes { get; set; }
    }
}
