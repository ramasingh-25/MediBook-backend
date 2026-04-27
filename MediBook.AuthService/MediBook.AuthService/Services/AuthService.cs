using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MediBook.AuthService.DTOs;
using MediBook.AuthService.Entities;
using MediBook.AuthService.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MediBook.AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponse> Register(RegisterRequest request)
        {
            // Check if email already exists
            bool emailExists = await _userRepository.ExistsByEmail(request.Email);
            if (emailExists)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Email is already registered."
                };
            }

            // Validate role
            if (request.Role != Roles.Patient && request.Role != Roles.Provider && request.Role != Roles.Admin)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Role must be Patient, Provider, or Admin."
                };
            }

            // Create new user
            var user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Phone = request.Phone,
                Role = request.Role,
                Provider = "Local",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                ProfilePicUrl = ""
            };

            await _userRepository.CreateUser(user);

            // Generate token
            string token = GenerateJwtToken(user);
            string refreshToken = GenerateRefreshToken();

            return new AuthResponse
            {
                Success = true,
                Message = "Registration successful.",
                Token = token,
                RefreshToken = refreshToken,
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<AuthResponse> Login(LoginRequest request)
        {
            var user = await _userRepository.FindByEmail(request.Email);

            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            if (!user.IsActive)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Your account has been deactivated. Please contact support."
                };
            }

            bool passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!passwordValid)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            string token = GenerateJwtToken(user);
            string refreshToken = GenerateRefreshToken();

            return new AuthResponse
            {
                Success = true,
                Message = "Login successful.",
                Token = token,
                RefreshToken = refreshToken,
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<bool> Logout(string userId)
        {
            // In a stateless JWT system, logout is handled client-side
            // Here we confirm the user exists
            var user = await _userRepository.FindByUserId(userId);
            return user != null;
        }

        public async Task<AuthResponse> RefreshToken(RefreshTokenRequest request)
        {
            // Validate the existing token and extract claims
            var principal = GetPrincipalFromExpiredToken(request.Token);
            if (principal == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid token."
                };
            }

            string userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userRepository.FindByUserId(userId);

            if (user == null || !user.IsActive)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "User not found or inactive."
                };
            }

            string newToken = GenerateJwtToken(user);
            string newRefreshToken = GenerateRefreshToken();

            return new AuthResponse
            {
                Success = true,
                Message = "Token refreshed.",
                Token = newToken,
                RefreshToken = newRefreshToken,
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<bool> ValidateToken(string token)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                var tokenHandler = new JwtSecurityTokenHandler();

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserProfileResponse> GetUserByEmail(string email)
        {
            var user = await _userRepository.FindByEmail(email);
            if (user == null) return null;
            return MapToProfileResponse(user);
        }

        public async Task<UserProfileResponse> GetUserById(string userId)
        {
            var user = await _userRepository.FindByUserId(userId);
            if (user == null) return null;
            return MapToProfileResponse(user);
        }

        public async Task<UserProfileResponse> UpdateProfile(string userId, UpdateProfileRequest request)
        {
            var user = await _userRepository.FindByUserId(userId);
            if (user == null) return null;

            user.FullName = request.FullName ?? user.FullName;
            user.Phone = request.Phone ?? user.Phone;
            user.ProfilePicUrl = request.ProfilePicUrl ?? user.ProfilePicUrl;

            await _userRepository.UpdateUser(user);
            return MapToProfileResponse(user);
        }

        public async Task<bool> ChangePassword(string userId, ChangePasswordRequest request)
        {
            var user = await _userRepository.FindByUserId(userId);
            if (user == null) return false;

            bool currentPasswordValid = BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash);
            if (!currentPasswordValid) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _userRepository.UpdateUser(user);
            return true;
        }

        public async Task<bool> DeactivateAccount(string userId)
        {
            var user = await _userRepository.FindByUserId(userId);
            if (user == null) return false;

            user.IsActive = false;
            await _userRepository.UpdateUser(user);
            return true;
        }

        // ─── Private Helpers ─────────────────────────────────────────

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(
                    int.Parse(_configuration["Jwt:ExpiryHours"])),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = false   // Allow expired tokens for refresh
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

                var jwtToken = securityToken as JwtSecurityToken;
                if (jwtToken == null || !jwtToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private UserProfileResponse MapToProfileResponse(User user)
        {
            return new UserProfileResponse
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                Provider = user.Provider,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                ProfilePicUrl = user.ProfilePicUrl
            };
        }
    }
}