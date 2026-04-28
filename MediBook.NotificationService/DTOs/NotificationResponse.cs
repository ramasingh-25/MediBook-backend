namespace MediBook.NotificationService.DTOs
{
    public class NotificationResponse
    {
        public string NotificationId { get; set; }
        public string RecipientId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Channel { get; set; }
        public string? RelatedId { get; set; }
        public string? RelatedType { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
    }
}
