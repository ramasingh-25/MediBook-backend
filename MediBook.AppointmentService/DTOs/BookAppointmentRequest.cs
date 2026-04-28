namespace MediBook.AppointmentService.DTOs
{
    public class BookAppointmentRequest
    {
        public string PatientId { get; set; }
        public string ProviderId { get; set; }
        public string SlotId { get; set; }
        public string ServiceType { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string ModeOfConsultation { get; set; }
        public string? Notes { get; set; }
    }
}
