using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediBook.AvailabilityService.DTOs;
using MediBook.AvailabilityService.Interfaces;

namespace MediBook.AvailabilityService.Controllers
{
    [ApiController]
    [Route("api/v1/slots")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpPost]
        public async Task<ActionResult<SlotResponse>> AddSlot([FromBody] AddSlotRequest request)
        {
            var response = await _scheduleService.AddSlot(request);
            return CreatedAtAction(nameof(GetSlotById), new { slotId = response.SlotId }, response);
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<List<SlotResponse>>> AddBulkSlots([FromBody] AddBulkSlotsRequest request)
        {
            var response = await _scheduleService.AddBulkSlots(request);
            return Ok(response);
        }

        [HttpPost("recurring")]
        public async Task<ActionResult<List<SlotResponse>>> GenerateRecurringSlots([FromBody] GenerateRecurringSlotsRequest request)
        {
            var response = await _scheduleService.GenerateRecurringSlots(request);
            return Ok(response);
        }

        [HttpGet("provider/{providerId}")]
        public async Task<ActionResult<List<SlotResponse>>> GetSlotsByProvider(string providerId)
        {
            var slots = await _scheduleService.GetSlotsByProvider(providerId);
            return Ok(slots);
        }

        [HttpGet("provider/{providerId}/available")]
        public async Task<ActionResult<List<SlotResponse>>> GetAvailableSlots(string providerId, [FromQuery] DateTime date)
        {
            var slots = await _scheduleService.GetAvailableSlots(providerId, date);
            return Ok(slots);
        }

        [HttpGet("{slotId}")]
        public async Task<ActionResult<SlotResponse>> GetSlotById(string slotId)
        {
            var slot = await _scheduleService.GetSlotById(slotId);
            if (slot == null)
            {
                return NotFound(new { message = "Slot not found." });
            }
            return Ok(slot);
        }

        [HttpPut("{slotId}/book")]
        public async Task<ActionResult<SlotResponse>> BookSlot(string slotId)
        {
            try
            {
                var slot = await _scheduleService.BookSlot(slotId);
                if (slot == null)
                {
                    return BadRequest(new { message = "Slot not found or not available for booking." });
                }
                return Ok(slot);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    message = "Failed to book slot", 
                    error = ex.Message,
                    innerError = ex.InnerException?.Message 
                });
            }
        }

        [HttpPut("{slotId}/block")]
        public async Task<ActionResult<SlotResponse>> BlockSlot(string slotId, [FromBody] BlockSlotRequest request)
        {
            var slot = await _scheduleService.BlockSlot(slotId, request);
            if (slot == null)
            {
                return NotFound(new { message = "Slot not found." });
            }
            return Ok(slot);
        }

        [HttpPut("{slotId}/unblock")]
        public async Task<ActionResult<SlotResponse>> UnblockSlot(string slotId)
        {
            var slot = await _scheduleService.UnblockSlot(slotId);
            if (slot == null)
            {
                return NotFound(new { message = "Slot not found." });
            }
            return Ok(slot);
        }

        [HttpPut("{slotId}")]
        public async Task<ActionResult<SlotResponse>> UpdateSlot(string slotId, [FromBody] UpdateSlotRequest request)
        {
            var slot = await _scheduleService.UpdateSlot(slotId, request);
            if (slot == null)
            {
                return NotFound(new { message = "Slot not found." });
            }
            return Ok(slot);
        }

        [HttpDelete("{slotId}")]
        public async Task<ActionResult> DeleteSlot(string slotId)
        {
            var result = await _scheduleService.DeleteSlot(slotId);
            if (!result)
            {
                return NotFound(new { message = "Slot not found." });
            }
            return NoContent();
        }
    }
}
