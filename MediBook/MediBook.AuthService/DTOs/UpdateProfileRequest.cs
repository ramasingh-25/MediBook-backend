using System.ComponentModel.DataAnnotations;

namespace MediBook.AuthService.DTOs;

public class UpdateProfileRequest
{
    [MaxLength(100)]
    public string? FullName { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    public string? ProfilePicUrl { get; set; }
}
