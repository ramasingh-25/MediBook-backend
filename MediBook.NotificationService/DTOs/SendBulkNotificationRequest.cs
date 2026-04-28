namespace MediBook.NotificationService.DTOs
{
    public class SendBulkNotificationRequest
    {
        public List<string> RecipientIds { get; set; }
        public string Type { get; set; } // BOOKING, REMINDER, CANCELLATION, PAYMENT, FOLLOWUP
        public string Title { get; set; }
        public string Message { get; set; }
        public string Channel { get; set; } // APP, EMAIL, SMS
        public string? RelatedId { get; set; }
        public string? RelatedType { get; set; }
    }
}
