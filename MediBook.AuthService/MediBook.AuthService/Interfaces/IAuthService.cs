using System.Threading.Tasks;
using MediBook.AuthService.DTOs;

namespace MediBook.AuthService.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> Register(RegisterRequest request);
        Task<AuthResponse> Login(LoginRequest request);
        Task<bool> Logout(string userId);
        Task<AuthResponse> RefreshToken(RefreshTokenRequest request);
        Task<bool> ValidateToken(string token);
        Task<UserProfileResponse> GetUserByEmail(string email);
        Task<UserProfileResponse> GetUserById(string userId);
        Task<UserProfileResponse> UpdateProfile(string userId, UpdateProfileRequest request);
        Task<bool> ChangePassword(string userId, ChangePasswordRequest request);
        Task<bool> DeactivateAccount(string userId);
    }
}