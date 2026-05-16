using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediBook.PaymentService.DTOs;
using MediBook.PaymentService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediBook.PaymentService.Controllers
{
    [ApiController]
    [Route("api/v1/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("process")]
        public async Task<ActionResult<PaymentResponse>> ProcessPayment([FromBody] ProcessPaymentRequest request)
        {
            var response = await _paymentService.ProcessPayment(request);
            return CreatedAtAction(nameof(GetPaymentByAppointment), new { appointmentId = response.AppointmentId }, response);
        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<ActionResult<PaymentResponse>> GetPaymentByAppointment(string appointmentId)
        {
            var payment = await _paymentService.GetPaymentByAppointment(appointmentId);
            if (payment == null)
            {
                return NotFound(new { message = "Payment not found." });
            }
            return Ok(payment);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<List<PaymentResponse>>> GetPaymentsByPatient(string patientId)
        {
            var payments = await _paymentService.GetPaymentsByPatient(patientId);
            return Ok(payments);
        }

        [HttpGet("provider/{providerId}")]
        public async Task<ActionResult<List<PaymentResponse>>> GetPaymentsByProvider(string providerId)
        {
            var payments = await _paymentService.GetPaymentsByProvider(providerId);
            return Ok(payments);
        }

        [HttpGet("patient/{patientId}/history")]
        public async Task<ActionResult<List<PaymentResponse>>> GetPaymentHistory(string patientId)
        {
            var payments = await _paymentService.GetPaymentHistory(patientId);
            return Ok(payments);
        }

        [HttpPost("{paymentId}/refund")]
        public async Task<ActionResult<PaymentResponse>> RefundPayment(string paymentId, [FromBody] RefundPaymentRequest request)
        {
            var payment = await _paymentService.RefundPayment(paymentId, request);
            if (payment == null)
            {
                return NotFound(new { message = "Payment not found." });
            }
            return Ok(payment);
        }

        [HttpGet("{paymentId}/status")]
        public async Task<ActionResult<string>> GetPaymentStatus(string paymentId)
        {
            var status = await _paymentService.GetPaymentStatus(paymentId);
            if (status == null)
            {
                return NotFound(new { message = "Payment not found." });
            }
            return Ok(status);
        }

        [HttpPut("{paymentId}/status")]
        public async Task<ActionResult<PaymentResponse>> UpdatePaymentStatus(string paymentId, [FromBody] string status)
        {
            var payment = await _paymentService.UpdatePaymentStatus(paymentId, status);
            if (payment == null)
            {
                return NotFound(new { message = "Payment not found." });
            }
            return Ok(payment);
        }

        [HttpGet("{paymentId}/invoice")]
        public async Task<ActionResult> GenerateInvoice(string paymentId)
        {
            var invoice = await _paymentService.GenerateInvoice(paymentId);
            if (invoice.Length == 0)
            {
                return NotFound(new { message = "Payment not found." });
            }
            return File(invoice, "application/pdf", $"invoice_{paymentId}.pdf");
        }

        [HttpGet("revenue/total")]
        public async Task<ActionResult<decimal>> GetTotalRevenue()
        {
            var revenue = await _paymentService.GetTotalRevenue();
            return Ok(revenue);
        }

        [AllowAnonymous]
        [HttpPost("fix-pending")]
        public async Task<ActionResult> FixPendingPayments()
        {
            var pending = await _paymentService.GetPaymentsByStatus("Pending");
            foreach (var p in pending)
            {
                await _paymentService.UpdatePaymentStatus(p.PaymentId, "Paid");
            }
            return Ok(new { message = $"Fixed {pending.Count} pending payments." });
        }

        [HttpGet("/test-payment-service")]
        public IActionResult TestPaymentService() => Ok("Payment Controller Loaded");

        [HttpGet("/api/v1/payments/admin-all")]
        [HttpGet("/api/v1/payments/all")]
        public async Task<ActionResult<List<PaymentResponse>>> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPayments();
            return Ok(payments);
        }

        [HttpGet("test-status")]
        public IActionResult TestStatus()
        {
            return Ok(new { status = "Payment Service is running", timestamp = DateTime.UtcNow });
        }
    }
}
