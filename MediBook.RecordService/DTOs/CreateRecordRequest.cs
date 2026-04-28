namespace MediBook.RecordService.DTOs
{
    public class CreateRecordRequest
    {
        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string ProviderId { get; set; }
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
        public string Notes { get; set; }
        public string? AttachmentUrl { get; set; }
        public DateTime? FollowUpDate { get; set; }
    }
}
