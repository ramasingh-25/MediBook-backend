namespace MediBook.AuthService.DTOs
{
    public class UpdateProfileRequest
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string ProfilePicUrl { get; set; }
    }
}