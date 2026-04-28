namespace MediBook.AppointmentService.Entities
{
    public class Appointment
    {
        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string ProviderId { get; set; }
        public string SlotId { get; set; }
        public string ServiceType { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } // Scheduled, Completed, Cancelled, No-Show
        public string? Notes { get; set; }
        public string ModeOfConsultation { get; set; } // In-Person, Teleconsultation
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
