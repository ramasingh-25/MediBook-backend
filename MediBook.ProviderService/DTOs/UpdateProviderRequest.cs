namespace MediBook.ProviderService.DTOs
{
    public class UpdateProviderRequest
    {
        public string? Specialization { get; set; }
        public string? Qualification { get; set; }
        public int? ExperienceYears { get; set; }
        public string? Bio { get; set; }
        public string? ClinicName { get; set; }
        public string? ClinicAddress { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
