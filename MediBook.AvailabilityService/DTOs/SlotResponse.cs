namespace MediBook.AvailabilityService.DTOs
{
    public class SlotResponse
    {
        public string SlotId { get; set; }
        public string ProviderId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsBooked { get; set; }
        public bool IsBlocked { get; set; }
        public string? Recurrence { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
