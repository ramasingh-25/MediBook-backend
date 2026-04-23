using MediBook.AuthService.DTOs;
using MediBook.AuthService.Entities;

namespace MediBook.AuthService.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<Entities.User?> GetUserByIdAsync(Guid userId);
    Task<AuthResponse> LogoutAsync();
    Task<AuthResponse> RefreshTokenAsync(RefreshRequest request);
    Task<AuthResponse> GetProfileAsync(Guid userId);
    Task<AuthResponse> UpdateProfileAsync(Guid userId, UpdateProfileRequest request);
    Task<AuthResponse> UpdatePasswordAsync(Guid userId, UpdatePasswordRequest request);
    Task<AuthResponse> DeactivateAccountAsync(Guid userId);
}
