using MediBook.PaymentService.Data;
using MediBook.PaymentService.Entities;
using MediBook.PaymentService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediBook.PaymentService.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _context;

        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<Payment?> FindByPaymentId(string paymentId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        public async Task<Payment?> FindByAppointmentId(string appointmentId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.AppointmentId == appointmentId);
        }

        public async Task<List<Payment>> FindByPatientId(string patientId)
        {
            return await _context.Payments
                .Where(p => p.PatientId == patientId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Payment>> FindByProviderId(string providerId)
        {
            return await _context.Payments
                .Where(p => p.ProviderId == providerId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Payment>> FindByStatus(string status)
        {
            return await _context.Payments
                .Where(p => p.Status == status)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Payment?> FindByTransactionId(string transactionId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.TransactionId == transactionId);
        }

        public async Task<decimal> SumAmountByPatientId(string patientId)
        {
            return await _context.Payments
                .Where(p => p.PatientId == patientId && p.Status == "Paid")
                .SumAsync(p => p.Amount);
        }

        public async Task<List<Payment>> FindByPaidAtBetween(DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Where(p => p.PaidAt >= startDate && p.PaidAt <= endDate)
                .OrderByDescending(p => p.PaidAt)
                .ToListAsync();
        }

        public async Task<Payment> CreatePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> UpdatePayment(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> DeletePayment(string paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null) return false;

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<List<Payment>> GetAll()
        {
            return await _context.Payments
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalRevenue()
        {
            return await _context.Payments
                .Where(p => p.Status == "Paid" || p.Status == "Completed" || p.Status == "Success")
                .SumAsync(p => p.Amount);
        }
    }
}
