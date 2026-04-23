using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MediBook.AuthService.DTOs;
using MediBook.AuthService.Entities;
using MediBook.AuthService.Repositories;

namespace MediBook.AuthService.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.EmailExistsAsync(request.Email))
        {
            return new AuthResponse { Success = false, Message = "Email already exists" };
        }

        var user = new User
        {
            UserId = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role ?? "Patient",
            Provider = "LOCAL",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user);

        var token = GenerateJwtToken(user);

        return new AuthResponse
        {
            Success = true,
            Token = token,
            User = new UserDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            }
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return new AuthResponse { Success = false, Message = "Invalid credentials" };
        }

        if (!user.IsActive)
        {
            return new AuthResponse { Success = false, Message = "Account is inactive" };
        }

        var token = GenerateJwtToken(user);

        return new AuthResponse
        {
            Success = true,
            Token = token,
            User = new UserDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            }
        };
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    public async Task<AuthResponse> LogoutAsync()
    {
        return new AuthResponse { Success = true, Message = "Logged out successfully" };
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshRequest request)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(request.Token);
        var userIdClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return new AuthResponse { Success = false, Message = "Invalid token" };
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null || !user.IsActive)
        {
            return new AuthResponse { Success = false, Message = "Invalid token" };
        }

        var token = GenerateJwtToken(user);
        return new AuthResponse
        {
            Success = true,
            Token = token,
            User = new UserDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            }
        };
    }

    public async Task<AuthResponse> GetProfileAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new AuthResponse { Success = false, Message = "User not found" };
        }

        return new AuthResponse
        {
            Success = true,
            User = new UserDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Phone = user.Phone,
                ProfilePicUrl = user.ProfilePicUrl,
                IsActive = user.IsActive
            }
        };
    }

    public async Task<AuthResponse> UpdateProfileAsync(Guid userId, UpdateProfileRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new AuthResponse { Success = false, Message = "User not found" };
        }

        if (!string.IsNullOrEmpty(request.FullName))
            user.FullName = request.FullName;
        if (!string.IsNullOrEmpty(request.Phone))
            user.Phone = request.Phone;
        if (!string.IsNullOrEmpty(request.ProfilePicUrl))
            user.ProfilePicUrl = request.ProfilePicUrl;

        await _userRepository.UpdateAsync(user);

        return new AuthResponse
        {
            Success = true,
            Message = "Profile updated successfully",
            User = new UserDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Phone = user.Phone,
                ProfilePicUrl = user.ProfilePicUrl
            }
        };
    }

    public async Task<AuthResponse> UpdatePasswordAsync(Guid userId, UpdatePasswordRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new AuthResponse { Success = false, Message = "User not found" };
        }

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
        {
            return new AuthResponse { Success = false, Message = "Current password is incorrect" };
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        await _userRepository.UpdateAsync(user);

        return new AuthResponse { Success = true, Message = "Password updated successfully" };
    }

    public async Task<AuthResponse> DeactivateAccountAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new AuthResponse { Success = false, Message = "User not found" };
        }

        user.IsActive = false;
        await _userRepository.UpdateAsync(user);

        return new AuthResponse { Success = true, Message = "Account deactivated successfully" };
    }

    private string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"]!;
        var jwtIssuer = _configuration["Jwt:Issuer"]!;
        var jwtAudience = _configuration["Jwt:Audience"]!;
        var jwtExpiryHours = int.Parse(_configuration["Jwt:ExpiryHours"]!);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(jwtExpiryHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
