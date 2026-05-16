namespace MediBook.AvailabilityService.DTOs
{
    public class AddBulkSlotsRequest
    {
        public string ProviderId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int DurationMinutes { get; set; }
    }
}
