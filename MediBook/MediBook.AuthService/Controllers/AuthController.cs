using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediBook.AuthService.DTOs;
using MediBook.AuthService.Services;

namespace MediBook.AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return StatusCode(500, new AuthResponse { Success = false, Message = "An error occurred during registration" });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);
            if (!result.Success)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return StatusCode(500, new AuthResponse { Success = false, Message = "An error occurred during login" });
        }
    }

    [HttpPost("logout")]
    public async Task<ActionResult<AuthResponse>> Logout()
    {
        try
        {
            var result = await _authService.LogoutAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return StatusCode(500, new AuthResponse { Success = false, Message = "An error occurred during logout" });
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshRequest request)
    {
        try
        {
            var result = await _authService.RefreshTokenAsync(request);
            if (!result.Success)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return StatusCode(500, new AuthResponse { Success = false, Message = "An error occurred during token refresh" });
        }
    }

    [HttpGet("profile")]
    public async Task<ActionResult<AuthResponse>> GetProfile([FromQuery] string userId)
    {
        try
        {
            if (Guid.TryParse(userId, out var userGuid))
            {
                var result = await _authService.GetProfileAsync(userGuid);
                if (!result.Success)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            return BadRequest(new AuthResponse { Success = false, Message = "Invalid user ID format" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting profile");
            return StatusCode(500, new AuthResponse { Success = false, Message = "An error occurred" });
        }
    }

    [HttpPut("profile")]
    public async Task<ActionResult<AuthResponse>> UpdateProfile([FromQuery] string userId, [FromBody] UpdateProfileRequest request)
    {
        try
        {
            if (Guid.TryParse(userId, out var userGuid))
            {
                var result = await _authService.UpdateProfileAsync(userGuid, request);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            return BadRequest(new AuthResponse { Success = false, Message = "Invalid user ID format" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profile");
            return StatusCode(500, new AuthResponse { Success = false, Message = "An error occurred" });
        }
    }

    [HttpPut("password")]
    public async Task<ActionResult<AuthResponse>> UpdatePassword([FromQuery] string userId, [FromBody] UpdatePasswordRequest request)
    {
        try
        {
            if (Guid.TryParse(userId, out var userGuid))
            {
                var result = await _authService.UpdatePasswordAsync(userGuid, request);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            return BadRequest(new AuthResponse { Success = false, Message = "Invalid user ID format" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating password");
            return StatusCode(500, new AuthResponse { Success = false, Message = "An error occurred" });
        }
    }

    [HttpDelete("deactivate")]
    public async Task<ActionResult<AuthResponse>> DeactivateAccount([FromQuery] string userId)
    {
        try
        {
            if (Guid.TryParse(userId, out var userGuid))
            {
                var result = await _authService.DeactivateAccountAsync(userGuid);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            return BadRequest(new AuthResponse { Success = false, Message = "Invalid user ID format" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating account");
            return StatusCode(500, new AuthResponse { Success = false, Message = "An error occurred" });
        }
    }
}
