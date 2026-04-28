using Microsoft.AspNetCore.Mvc;
using MediBook.RecordService.DTOs;
using MediBook.RecordService.Interfaces;

namespace MediBook.RecordService.Controllers
{
    [ApiController]
    [Route("api/v1/records")]
    public class RecordController : ControllerBase
    {
        private readonly IRecordService _recordService;

        public RecordController(IRecordService recordService)
        {
            _recordService = recordService;
        }

        [HttpPost]
        public async Task<ActionResult<RecordResponse>> CreateRecord([FromBody] CreateRecordRequest request)
        {
            try
            {
                var response = await _recordService.CreateRecord(request);
                return CreatedAtAction(nameof(GetRecordById), new { recordId = response.RecordId }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<ActionResult<RecordResponse>> GetRecordByAppointment(string appointmentId)
        {
            var record = await _recordService.GetRecordByAppointment(appointmentId);
            if (record == null)
            {
                return NotFound(new { message = "Medical record not found for this appointment." });
            }
            return Ok(record);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<List<RecordResponse>>> GetRecordsByPatient(string patientId)
        {
            var records = await _recordService.GetRecordsByPatient(patientId);
            return Ok(records);
        }

        [HttpGet("provider/{providerId}")]
        public async Task<ActionResult<List<RecordResponse>>> GetRecordsByProvider(string providerId)
        {
            var records = await _recordService.GetRecordsByProvider(providerId);
            return Ok(records);
        }

        [HttpGet("{recordId}")]
        public async Task<ActionResult<RecordResponse>> GetRecordById(string recordId)
        {
            var record = await _recordService.GetRecordById(recordId);
            if (record == null)
            {
                return NotFound(new { message = "Medical record not found." });
            }
            return Ok(record);
        }

        [HttpPut("{recordId}")]
        public async Task<ActionResult<RecordResponse>> UpdateRecord(string recordId, [FromBody] UpdateRecordRequest request)
        {
            var record = await _recordService.UpdateRecord(recordId, request);
            if (record == null)
            {
                return NotFound(new { message = "Medical record not found." });
            }
            return Ok(record);
        }

        [HttpGet("followUps/{followUpDate}")]
        public async Task<ActionResult<List<RecordResponse>>> GetFollowUpRecords(DateTime followUpDate)
        {
            var records = await _recordService.GetFollowUpRecords(followUpDate);
            return Ok(records);
        }

        [HttpGet("patient/{patientId}/count")]
        public async Task<ActionResult<int>> GetRecordCount(string patientId)
        {
            var count = await _recordService.GetRecordCount(patientId);
            return Ok(count);
        }

        [HttpDelete("{recordId}")]
        public async Task<ActionResult<bool>> DeleteRecord(string recordId)
        {
            var result = await _recordService.DeleteRecord(recordId);
            if (!result)
            {
                return NotFound(new { message = "Medical record not found." });
            }
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<RecordResponse>>> GetAllRecords()
        {
            var records = await _recordService.GetAllRecords();
            return Ok(records);
        }
    }
}
