using MediBook.PaymentService.DTOs;
using MediBook.PaymentService.Entities;
using MediBook.PaymentService.Interfaces;
using RazorpayClient = Razorpay.Api;

namespace MediBook.PaymentService.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repository;
        private readonly IConfiguration _configuration;

        public PaymentService(IPaymentRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<PaymentResponse> ProcessPayment(ProcessPaymentRequest request)
        {
            var payment = new Payment
            {
                PaymentId = Guid.NewGuid().ToString(),
                AppointmentId = request.AppointmentId,
                PatientId = request.PatientId,
                Amount = request.Amount,
                Status = "Pending",
                Mode = request.Mode,
                TransactionId = request.RazorpayPaymentId ?? Guid.NewGuid().ToString(),
                Currency = request.Currency,
                PaidAt = null,
                RefundedAt = null,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                // Initialize Razorpay client
                var keyId = _configuration["Razorpay:KeyId"];
                var keySecret = _configuration["Razorpay:KeySecret"];
                
                if (string.IsNullOrEmpty(keyId) || string.IsNullOrEmpty(keySecret))
                {
                    payment.Status = "Failed";
                    payment.Notes = "Razorpay configuration missing";
                    var createdPayment = await _repository.CreatePayment(payment);
                    return MapToResponse(createdPayment);
                }

                // If Razorpay payment ID is provided, verify the payment
                if (!string.IsNullOrEmpty(request.RazorpayPaymentId))
                {
                    var client = new RazorpayClient.RazorpayClient(keyId, keySecret);
                    var razorpayPayment = client.Payment.Fetch(request.RazorpayPaymentId);

                    if (razorpayPayment["status"] == "captured")
                    {
                        payment.Status = "Paid";
                        payment.PaidAt = DateTime.UtcNow;
                        payment.TransactionId = request.RazorpayPaymentId;
                    }
                    else
                    {
                        payment.Status = "Failed";
                        payment.Notes = $"Payment status: {razorpayPayment["status"]}";
                    }
                }
                else
                {
                    // Create a Razorpay order
                    var client = new RazorpayClient.RazorpayClient(keyId, keySecret);
                    var options = new Dictionary<string, object>
                    {
                        { "amount", request.Amount * 100 }, // Razorpay expects amount in paise
                        { "currency", request.Currency },
                        { "receipt", payment.PaymentId },
                        { "payment_capture", 1 }
                    };
                    
                    var order = client.Order.Create(options);
                    payment.Notes = $"Order created: {order["id"]}";
                    payment.TransactionId = order["id"].ToString();
                }
            }
            catch (Exception ex)
            {
                payment.Status = "Failed";
                payment.Notes = $"Error: {ex.Message}";
            }

            var finalPayment = await _repository.CreatePayment(payment);
            return MapToResponse(finalPayment);
        }

        public async Task<PaymentResponse?> GetPaymentByAppointment(string appointmentId)
        {
            var payment = await _repository.FindByAppointmentId(appointmentId);
            return payment == null ? null : MapToResponse(payment);
        }

        public async Task<List<PaymentResponse>> GetPaymentsByPatient(string patientId)
        {
            var payments = await _repository.FindByPatientId(patientId);
            return payments.Select(MapToResponse).ToList();
        }

        public async Task<List<PaymentResponse>> GetPaymentHistory(string patientId)
        {
            var payments = await _repository.FindByPatientId(patientId);
            return payments.Select(MapToResponse).ToList();
        }

        public async Task<PaymentResponse?> RefundPayment(string paymentId, RefundPaymentRequest request)
        {
            var payment = await _repository.FindByPaymentId(paymentId);
            if (payment == null) return null;

            try
            {
                var keyId = _configuration["Razorpay:KeyId"];
                var keySecret = _configuration["Razorpay:KeySecret"];
                
                if (!string.IsNullOrEmpty(payment.TransactionId) && payment.Status == "Paid")
                {
                    var client = new RazorpayClient.RazorpayClient(keyId, keySecret);
                    var refundOptions = new Dictionary<string, object>
                    {
                        { "amount", payment.Amount * 100 }
                    };
                    
                    var razorpayPayment = client.Payment.Fetch(payment.TransactionId);
                    var refund = razorpayPayment.Refund(refundOptions);
                    payment.Status = "Refunded";
                    payment.RefundedAt = DateTime.UtcNow;
                    payment.Notes = request.Reason ?? $"Refund ID: {refund["id"]}";
                }
                else
                {
                    payment.Status = "Refunded";
                    payment.RefundedAt = DateTime.UtcNow;
                    payment.Notes = request.Reason;
                }
            }
            catch (Exception ex)
            {
                payment.Notes = $"Refund error: {ex.Message}";
            }

            var updatedPayment = await _repository.UpdatePayment(payment);
            return MapToResponse(updatedPayment);
        }

        public async Task<string?> GetPaymentStatus(string paymentId)
        {
            var payment = await _repository.FindByPaymentId(paymentId);
            return payment?.Status;
        }

        public async Task<PaymentResponse?> UpdatePaymentStatus(string paymentId, string status)
        {
            var payment = await _repository.FindByPaymentId(paymentId);
            if (payment == null) return null;

            payment.Status = status;
            if (status == "Paid")
            {
                payment.PaidAt = DateTime.UtcNow;
            }
            var updatedPayment = await _repository.UpdatePayment(payment);
            return MapToResponse(updatedPayment);
        }

        public async Task<byte[]> GenerateInvoice(string paymentId)
        {
            // Placeholder for invoice generation
            // In a real implementation, this would use QuestPDF or similar library
            var payment = await _repository.FindByPaymentId(paymentId);
            if (payment == null) return Array.Empty<byte>();

            // Simple text-based invoice for now
            var invoiceText = $"INVOICE\nPayment ID: {payment.PaymentId}\n" +
                             $"Appointment ID: {payment.AppointmentId}\n" +
                             $"Amount: {payment.Amount} {payment.Currency}\n" +
                             $"Status: {payment.Status}\n" +
                             $"Transaction ID: {payment.TransactionId}\n" +
                             $"Date: {payment.PaidAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A"}";
            return System.Text.Encoding.UTF8.GetBytes(invoiceText);
        }

        public async Task<decimal> GetTotalRevenue()
        {
            // This would need to be implemented with proper aggregation
            // For now, return a placeholder
            return 0;
        }

        private PaymentResponse MapToResponse(Payment payment)
        {
            return new PaymentResponse
            {
                PaymentId = payment.PaymentId,
                AppointmentId = payment.AppointmentId,
                PatientId = payment.PatientId,
                Amount = payment.Amount,
                Status = payment.Status,
                Mode = payment.Mode,
                TransactionId = payment.TransactionId,
                Currency = payment.Currency,
                PaidAt = payment.PaidAt,
                RefundedAt = payment.RefundedAt,
                Notes = payment.Notes,
                CreatedAt = payment.CreatedAt
            };
        }
    }
}
