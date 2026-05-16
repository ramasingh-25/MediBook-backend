namespace MediBook.RecordService.DTOs
{
    public class UpdateRecordRequest
    {
        public string? Diagnosis { get; set; }
        public string? Prescription { get; set; }
        public string? Notes { get; set; }
        public string? AttachmentUrl { get; set; }
        public DateTime? FollowUpDate { get; set; }
    }
}
