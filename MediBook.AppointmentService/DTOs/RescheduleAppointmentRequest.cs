namespace MediBook.AppointmentService.DTOs
{
    public class RescheduleAppointmentRequest
    {
        public string NewSlotId { get; set; }
        public DateTime NewAppointmentDate { get; set; }
        public TimeSpan NewStartTime { get; set; }
        public TimeSpan NewEndTime { get; set; }
    }
}
