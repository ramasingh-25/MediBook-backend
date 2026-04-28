namespace MediBook.ReviewService.Entities
{
    public class Review
    {
        public string ReviewId { get; set; }
        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string ProviderId { get; set; }
        public int Rating { get; set; } // 1-5
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool IsVerified { get; set; }
        public bool IsAnonymous { get; set; }
    }
}
