using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediBook.AppointmentService.DTOs;
using MediBook.AppointmentService.Interfaces;

namespace MediBook.AppointmentService.Controllers
{
    [ApiController]
    [Route("api/v1/appointments")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public async Task<ActionResult> BookAppointment([FromBody] BookAppointmentRequest request)
        {
            try
            {
                var response = await _appointmentService.BookAppointment(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    message = "Failed to book appointment", 
                    error = ex.Message,
                    innerError = ex.InnerException?.Message 
                });
            }
        }

        [HttpGet("{appointmentId}")]
        public async Task<ActionResult<AppointmentResponse>> GetById(string appointmentId)
        {
            var appointment = await _appointmentService.GetById(appointmentId);
            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found." });
            }
            return Ok(appointment);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<List<AppointmentResponse>>> GetByPatient(string patientId)
        {
            var appointments = await _appointmentService.GetByPatient(patientId);
            return Ok(appointments);
        }

        [HttpGet("patient/{patientId}/upcoming")]
        public async Task<ActionResult<List<AppointmentResponse>>> GetUpcomingByPatient(string patientId)
        {
            var appointments = await _appointmentService.GetUpcomingByPatient(patientId);
            return Ok(appointments);
        }

        [HttpGet("provider/{providerId}")]
        public async Task<ActionResult<List<AppointmentResponse>>> GetByProvider(string providerId)
        {
            var appointments = await _appointmentService.GetByProvider(providerId);
            return Ok(appointments);
        }

        [HttpGet("provider/{providerId}/date/{date}")]
        public async Task<ActionResult<List<AppointmentResponse>>> GetByProviderAndDate(string providerId, DateTime date)
        {
            var appointments = await _appointmentService.GetByProviderAndDate(providerId, date);
            return Ok(appointments);
        }

        [HttpPut("{appointmentId}/cancel")]
        public async Task<ActionResult<AppointmentResponse>> CancelAppointment(string appointmentId)
        {
            var appointment = await _appointmentService.CancelAppointment(appointmentId);
            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found." });
            }
            return Ok(appointment);
        }

        [HttpPut("{appointmentId}/reschedule")]
        public async Task<ActionResult<AppointmentResponse>> RescheduleAppointment(string appointmentId, [FromBody] RescheduleAppointmentRequest request)
        {
            var appointment = await _appointmentService.RescheduleAppointment(appointmentId, request);
            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found." });
            }
            return Ok(appointment);
        }

        [HttpPut("{appointmentId}/complete")]
        public async Task<ActionResult<AppointmentResponse>> CompleteAppointment(string appointmentId)
        {
            var appointment = await _appointmentService.CompleteAppointment(appointmentId);
            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found." });
            }
            return Ok(appointment);
        }

        [HttpPut("{appointmentId}/status")]
        public async Task<ActionResult<AppointmentResponse>> UpdateStatus(string appointmentId, [FromBody] string status)
        {
            var appointment = await _appointmentService.UpdateStatus(appointmentId, status);
            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found." });
            }
            return Ok(appointment);
        }

        [HttpGet("provider/{providerId}/count")]
        public async Task<ActionResult<int>> GetAppointmentCount(string providerId)
        {
            var count = await _appointmentService.GetAppointmentCount(providerId);
            return Ok(count);
        }
    }
}
