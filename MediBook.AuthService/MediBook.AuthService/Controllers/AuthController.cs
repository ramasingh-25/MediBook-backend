using System.Security.Claims;
using System.Threading.Tasks;
using MediBook.AuthService.DTOs;
using MediBook.AuthService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediBook.AuthService.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST /api/v1/auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null)
                return BadRequest(new { Message = "Request body is required." });

            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new { Message = "Email is required." });

            if (string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { Message = "Password is required." });

            if (string.IsNullOrWhiteSpace(request.FullName))
                return BadRequest(new { Message = "Full name is required." });

            var result = await _authService.Register(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // POST /api/v1/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
                return BadRequest(new { Message = "Request body is required." });

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { Message = "Email and password are required." });

            var result = await _authService.Login(request);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        // POST /api/v1/auth/logout
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _authService.Logout(userId);

            return Ok(new { Message = "Logged out successfully." });
        }

        // POST /api/v1/auth/refresh
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Token))
                return BadRequest(new { Message = "Token is required." });

            var result = await _authService.RefreshToken(request);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        // GET /api/v1/auth/profile
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _authService.GetUserById(userId);

            if (profile == null)
                return NotFound(new { Message = "User not found." });

            return Ok(profile);
        }

        // PUT /api/v1/auth/profile
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            if (request == null)
                return BadRequest(new { Message = "Request body is required." });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _authService.UpdateProfile(userId, request);

            if (result == null)
                return NotFound(new { Message = "User not found." });

            return Ok(result);
        }

        // PUT /api/v1/auth/password
        [HttpPut("password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (request == null)
                return BadRequest(new { Message = "Request body is required." });

            if (string.IsNullOrWhiteSpace(request.CurrentPassword) ||
                string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return BadRequest(new { Message = "Current and new password are required." });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool result = await _authService.ChangePassword(userId, request);

            if (!result)
                return BadRequest(new { Message = "Current password is incorrect." });

            return Ok(new { Message = "Password changed successfully." });
        }

        // DELETE /api/v1/auth/deactivate
        [HttpDelete("deactivate")]
        [Authorize]
        public async Task<IActionResult> DeactivateAccount()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool result = await _authService.DeactivateAccount(userId);

            if (!result)
                return NotFound(new { Message = "User not found." });

            return Ok(new { Message = "Account deactivated successfully." });
        }
    }
}