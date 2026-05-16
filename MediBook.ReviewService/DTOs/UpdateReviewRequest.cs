namespace MediBook.ReviewService.DTOs
{
    public class UpdateReviewRequest
    {
        public int Rating { get; set; } // 1-5
        public string? Comment { get; set; }
        public bool IsAnonymous { get; set; }
    }
}
