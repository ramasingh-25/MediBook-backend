using MediBook.PaymentService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediBook.PaymentService.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> FindByPaymentId(string paymentId);
        Task<Payment?> FindByAppointmentId(string appointmentId);
        Task<List<Payment>> FindByPatientId(string patientId);
        Task<List<Payment>> FindByProviderId(string providerId);
        Task<List<Payment>> FindByStatus(string status);
        Task<Payment?> FindByTransactionId(string transactionId);
        Task<decimal> SumAmountByPatientId(string patientId);
        Task<List<Payment>> FindByPaidAtBetween(DateTime startDate, DateTime endDate);
        Task<Payment> CreatePayment(Payment payment);
        Task<Payment> UpdatePayment(Payment payment);
        Task<bool> DeletePayment(string paymentId);
        Task<List<Payment>> GetAll();
        Task<decimal> GetTotalRevenue();
    }
}
