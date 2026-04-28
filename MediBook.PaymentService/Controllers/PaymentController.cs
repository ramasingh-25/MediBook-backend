using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediBook.PaymentService.DTOs;
using MediBook.PaymentService.Interfaces;

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
    }
}
