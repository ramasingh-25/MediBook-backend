using MediBook.PaymentService.DTOs;

namespace MediBook.PaymentService.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponse> ProcessPayment(ProcessPaymentRequest request);
        Task<PaymentResponse?> GetPaymentByAppointment(string appointmentId);
        Task<List<PaymentResponse>> GetPaymentsByPatient(string patientId);
        Task<List<PaymentResponse>> GetPaymentHistory(string patientId);
        Task<PaymentResponse?> RefundPayment(string paymentId, RefundPaymentRequest request);
        Task<string?> GetPaymentStatus(string paymentId);
        Task<PaymentResponse?> UpdatePaymentStatus(string paymentId, string status);
        Task<byte[]> GenerateInvoice(string paymentId);
        Task<decimal> GetTotalRevenue();
    }
}
