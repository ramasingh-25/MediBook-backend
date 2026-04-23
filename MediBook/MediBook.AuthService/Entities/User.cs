using System.ComponentModel.DataAnnotations;

namespace MediBook.AuthService.Entities
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        // Patient | Provider | Admin
        [Required]
        public string Role { get; set; } = "Patient";

        // LOCAL | GOOGLE | GITHUB
        public string? Provider { get; set; }

        public bool IsActive { get; set; } = true;

        public string? ProfilePicUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}