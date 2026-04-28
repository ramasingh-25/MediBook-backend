namespace MediBook.PaymentService.DTOs
{
    public class ProcessPaymentRequest
    {
        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public decimal Amount { get; set; }
        public string Mode { get; set; } // Card, UPI, Wallet
        public string Currency { get; set; }
        public string? Notes { get; set; }
        public string? RazorpayPaymentId { get; set; }
        public string? RazorpayOrderId { get; set; }
        public string? RazorpaySignature { get; set; }
    }
}
