using MediBook.AppointmentService.Data;
using MediBook.AppointmentService.Entities;
using MediBook.AppointmentService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediBook.AppointmentService.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppointmentDbContext _context;

        public AppointmentRepository(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment?> FindByAppointmentId(string appointmentId)
        {
            return await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task<List<Appointment>> FindByPatientId(string patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<List<Appointment>> FindByProviderId(string providerId)
        {
            return await _context.Appointments
                .Where(a => a.ProviderId == providerId)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<Appointment?> FindBySlotId(string slotId)
        {
            return await _context.Appointments.FirstOrDefaultAsync(a => a.SlotId == slotId);
        }

        public async Task<List<Appointment>> FindByStatus(string status)
        {
            return await _context.Appointments
                .Where(a => a.Status == status)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<List<Appointment>> FindByProviderIdAndAppointmentDate(string providerId, DateTime date)
        {
            var targetDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
            return await _context.Appointments
                .Where(a => a.ProviderId == providerId && a.AppointmentDate >= targetDate && a.AppointmentDate < targetDate.AddDays(1))
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<List<Appointment>> FindUpcomingByPatientId(string patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId && 
                           a.AppointmentDate >= DateTime.UtcNow &&
                           a.Status == "Scheduled")
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<int> CountByProviderId(string providerId)
        {
            return await _context.Appointments
                .CountAsync(a => a.ProviderId == providerId);
        }

        public async Task<Appointment> CreateAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment> UpdateAppointment(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<bool> DeleteAppointment(string appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null) return false;

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
