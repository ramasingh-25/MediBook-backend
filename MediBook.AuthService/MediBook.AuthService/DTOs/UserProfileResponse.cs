using System;

namespace MediBook.AuthService.DTOs
{
    public class UserProfileResponse
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Provider { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ProfilePicUrl { get; set; }
    }
}