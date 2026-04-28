namespace MediBook.AppointmentService.DTOs
{
    public class AppointmentResponse
    {
        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string ProviderId { get; set; }
        public string SlotId { get; set; }
        public string ServiceType { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; }
        public string? Notes { get; set; }
        public string ModeOfConsultation { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
