namespace MediBook.ProviderService.DTOs
{
    public class RegisterProviderRequest
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Specialization { get; set; }
        public string Qualification { get; set; }
        public int ExperienceYears { get; set; }
        public string Bio { get; set; }
        public string ClinicName { get; set; }
        public string ClinicAddress { get; set; }
    }
}
