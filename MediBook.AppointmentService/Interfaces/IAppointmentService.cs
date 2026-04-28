using MediBook.AppointmentService.DTOs;

namespace MediBook.AppointmentService.Interfaces
{
    public interface IAppointmentService
    {
        Task<AppointmentResponse> BookAppointment(BookAppointmentRequest request);
        Task<AppointmentResponse?> GetById(string appointmentId);
        Task<List<AppointmentResponse>> GetByPatient(string patientId);
        Task<List<AppointmentResponse>> GetByProvider(string providerId);
        Task<List<AppointmentResponse>> GetByProviderAndDate(string providerId, DateTime date);
        Task<AppointmentResponse?> CancelAppointment(string appointmentId);
        Task<AppointmentResponse?> RescheduleAppointment(string appointmentId, RescheduleAppointmentRequest request);
        Task<AppointmentResponse?> CompleteAppointment(string appointmentId);
        Task<AppointmentResponse?> UpdateStatus(string appointmentId, string status);
        Task<List<AppointmentResponse>> GetUpcomingByPatient(string patientId);
        Task<int> GetAppointmentCount(string providerId);
    }
}
