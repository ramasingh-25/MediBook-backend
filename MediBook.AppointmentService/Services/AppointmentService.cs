using MediBook.AppointmentService.DTOs;
using MediBook.AppointmentService.Entities;
using MediBook.AppointmentService.Interfaces;

namespace MediBook.AppointmentService.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentService(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<AppointmentResponse> BookAppointment(BookAppointmentRequest request)
        {
            var appointment = new Appointment
            {
                AppointmentId = Guid.NewGuid().ToString(),
                PatientId = request.PatientId,
                ProviderId = request.ProviderId,
                SlotId = request.SlotId,
                ServiceType = request.ServiceType,
                AppointmentDate = request.AppointmentDate,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Status = "Scheduled",
                Notes = request.Notes,
                ModeOfConsultation = request.ModeOfConsultation,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdAppointment = await _repository.CreateAppointment(appointment);
            return MapToResponse(createdAppointment);
        }

        public async Task<AppointmentResponse?> GetById(string appointmentId)
        {
            var appointment = await _repository.FindByAppointmentId(appointmentId);
            return appointment == null ? null : MapToResponse(appointment);
        }

        public async Task<List<AppointmentResponse>> GetByPatient(string patientId)
        {
            var appointments = await _repository.FindByPatientId(patientId);
            return appointments.Select(MapToResponse).ToList();
        }

        public async Task<List<AppointmentResponse>> GetByProvider(string providerId)
        {
            var appointments = await _repository.FindByProviderId(providerId);
            return appointments.Select(MapToResponse).ToList();
        }

        public async Task<List<AppointmentResponse>> GetByProviderAndDate(string providerId, DateTime date)
        {
            var appointments = await _repository.FindByProviderIdAndAppointmentDate(providerId, date);
            return appointments.Select(MapToResponse).ToList();
        }

        public async Task<AppointmentResponse?> CancelAppointment(string appointmentId)
        {
            var appointment = await _repository.FindByAppointmentId(appointmentId);
            if (appointment == null) return null;

            appointment.Status = "Cancelled";
            appointment.UpdatedAt = DateTime.UtcNow;
            var updatedAppointment = await _repository.UpdateAppointment(appointment);
            return MapToResponse(updatedAppointment);
        }

        public async Task<AppointmentResponse?> RescheduleAppointment(string appointmentId, RescheduleAppointmentRequest request)
        {
            var appointment = await _repository.FindByAppointmentId(appointmentId);
            if (appointment == null) return null;

            appointment.SlotId = request.NewSlotId;
            appointment.AppointmentDate = request.NewAppointmentDate;
            appointment.StartTime = request.NewStartTime;
            appointment.EndTime = request.NewEndTime;
            appointment.UpdatedAt = DateTime.UtcNow;
            var updatedAppointment = await _repository.UpdateAppointment(appointment);
            return MapToResponse(updatedAppointment);
        }

        public async Task<AppointmentResponse?> CompleteAppointment(string appointmentId)
        {
            var appointment = await _repository.FindByAppointmentId(appointmentId);
            if (appointment == null) return null;

            appointment.Status = "Completed";
            appointment.UpdatedAt = DateTime.UtcNow;
            var updatedAppointment = await _repository.UpdateAppointment(appointment);
            return MapToResponse(updatedAppointment);
        }

        public async Task<AppointmentResponse?> UpdateStatus(string appointmentId, string status)
        {
            var appointment = await _repository.FindByAppointmentId(appointmentId);
            if (appointment == null) return null;

            appointment.Status = status;
            appointment.UpdatedAt = DateTime.UtcNow;
            var updatedAppointment = await _repository.UpdateAppointment(appointment);
            return MapToResponse(updatedAppointment);
        }

        public async Task<List<AppointmentResponse>> GetUpcomingByPatient(string patientId)
        {
            var appointments = await _repository.FindUpcomingByPatientId(patientId);
            return appointments.Select(MapToResponse).ToList();
        }

        public async Task<int> GetAppointmentCount(string providerId)
        {
            return await _repository.CountByProviderId(providerId);
        }

        private AppointmentResponse MapToResponse(Appointment appointment)
        {
            return new AppointmentResponse
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                ProviderId = appointment.ProviderId,
                SlotId = appointment.SlotId,
                ServiceType = appointment.ServiceType,
                AppointmentDate = appointment.AppointmentDate,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Status = appointment.Status,
                Notes = appointment.Notes,
                ModeOfConsultation = appointment.ModeOfConsultation,
                CreatedAt = appointment.CreatedAt,
                UpdatedAt = appointment.UpdatedAt
            };
        }
    }
}
