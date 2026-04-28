namespace MediBook.ReviewService.DTOs
{
    public class AddReviewRequest
    {
        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string ProviderId { get; set; }
        public int Rating { get; set; } // 1-5
        public string? Comment { get; set; }
        public bool IsAnonymous { get; set; }
    }
}
