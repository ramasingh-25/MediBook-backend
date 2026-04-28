namespace MediBook.PaymentService.DTOs
{
    public class PaymentResponse
    {
        public string PaymentId { get; set; }
        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Mode { get; set; }
        public string? TransactionId { get; set; }
        public string Currency { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? RefundedAt { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
