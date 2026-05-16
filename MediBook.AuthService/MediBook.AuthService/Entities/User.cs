using System;

namespace MediBook.AuthService.Entities
{
    public class User
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }           // Patient, Provider, Admin
        public string Provider { get; set; }       // Local, Google, GitHub
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ProfilePicUrl { get; set; }
    }
}