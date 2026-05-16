using MediBook.AppointmentService.Entities;

namespace MediBook.AppointmentService.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> FindByAppointmentId(string appointmentId);
        Task<List<Appointment>> FindByPatientId(string patientId);
        Task<List<Appointment>> FindByProviderId(string providerId);
        Task<Appointment?> FindBySlotId(string slotId);
        Task<List<Appointment>> FindByStatus(string status);
        Task<List<Appointment>> FindByProviderIdAndAppointmentDate(string providerId, DateTime date);
        Task<List<Appointment>> FindUpcomingByPatientId(string patientId);
        Task<int> CountByProviderId(string providerId);
        Task<Appointment> CreateAppointment(Appointment appointment);
        Task<Appointment> UpdateAppointment(Appointment appointment);
        Task<bool> DeleteAppointment(string appointmentId);
    }
}
