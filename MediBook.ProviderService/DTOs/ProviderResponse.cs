namespace MediBook.ProviderService.DTOs
{
    public class ProviderResponse
    {
        public string ProviderId { get; set; }
        public string UserId { get; set; }
        public string Specialization { get; set; }
        public string Qualification { get; set; }
        public int ExperienceYears { get; set; }
        public string Bio { get; set; }
        public string ClinicName { get; set; }
        public string ClinicAddress { get; set; }
        public double AvgRating { get; set; }
        public bool IsVerified { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
