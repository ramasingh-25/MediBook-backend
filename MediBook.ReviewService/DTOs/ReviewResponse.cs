namespace MediBook.ReviewService.DTOs
{
    public class ReviewResponse
    {
        public string ReviewId { get; set; }
        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string ProviderId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool IsVerified { get; set; }
        public bool IsAnonymous { get; set; }
    }
}
