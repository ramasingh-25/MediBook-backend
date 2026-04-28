using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediBook.ProviderService.DTOs;
using MediBook.ProviderService.Interfaces;

namespace MediBook.ProviderService.Controllers
{
    [ApiController]
    [Route("api/v1/providers")]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService _providerService;

        public ProviderController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpPost]
        public async Task<ActionResult<ProviderResponse>> RegisterProvider([FromBody] RegisterProviderRequest request)
        {
            try
            {
                var response = await _providerService.RegisterProvider(request);
                return CreatedAtAction(nameof(GetProviderById), new { providerId = response.ProviderId }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{providerId}")]
        public async Task<ActionResult<ProviderResponse>> GetProviderById(string providerId)
        {
            var provider = await _providerService.GetProviderById(providerId);
            if (provider == null)
            {
                return NotFound(new { message = "Provider not found." });
            }
            return Ok(provider);
        }

        [HttpGet("specialization/{specialization}")]
        public async Task<ActionResult<List<ProviderResponse>>> GetBySpecialization(string specialization)
        {
            var providers = await _providerService.GetBySpecialization(specialization);
            return Ok(providers);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<ProviderResponse>>> SearchProviders([FromQuery] string term)
        {
            var providers = await _providerService.SearchProviders(term);
            return Ok(providers);
        }

        [HttpGet]
        public async Task<ActionResult<List<ProviderResponse>>> GetAllProviders()
        {
            var providers = await _providerService.GetAllProviders();
            return Ok(providers);
        }

        [HttpPut("{providerId}")]
        public async Task<ActionResult<ProviderResponse>> UpdateProvider(string providerId, [FromBody] UpdateProviderRequest request)
        {
            var provider = await _providerService.UpdateProvider(providerId, request);
            if (provider == null)
            {
                return NotFound(new { message = "Provider not found." });
            }
            return Ok(provider);
        }

        [HttpPut("{providerId}/verify")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<ProviderResponse>> VerifyProvider(string providerId)
        {
            var provider = await _providerService.VerifyProvider(providerId);
            if (provider == null)
            {
                return NotFound(new { message = "Provider not found." });
            }
            return Ok(provider);
        }

        [HttpPut("{providerId}/availability")]
        public async Task<ActionResult<ProviderResponse>> SetAvailability(string providerId, [FromBody] SetAvailabilityRequest request)
        {
            var provider = await _providerService.SetAvailability(providerId, request);
            if (provider == null)
            {
                return NotFound(new { message = "Provider not found." });
            }
            return Ok(provider);
        }

        [HttpDelete("{providerId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> DeleteProvider(string providerId)
        {
            var result = await _providerService.DeleteProvider(providerId);
            if (!result)
            {
                return NotFound(new { message = "Provider not found." });
            }
            return NoContent();
        }

        [HttpPut("{providerId}/rating")]
        public async Task<ActionResult<ProviderResponse>> UpdateRating(string providerId, [FromBody] double newRating)
        {
            var provider = await _providerService.UpdateRating(providerId, newRating);
            if (provider == null)
            {
                return NotFound(new { message = "Provider not found." });
            }
            return Ok(provider);
        }
    }
}
