namespace MediBook.AvailabilityService.DTOs
{
    public class UpdateSlotRequest
    {
        public DateTime? Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? DurationMinutes { get; set; }
    }
}
