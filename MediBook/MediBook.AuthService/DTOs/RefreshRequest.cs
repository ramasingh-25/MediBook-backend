using System.ComponentModel.DataAnnotations;

namespace MediBook.AuthService.DTOs
{
    public class RefreshRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
