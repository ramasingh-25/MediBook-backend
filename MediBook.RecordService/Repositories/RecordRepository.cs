using MediBook.RecordService.Data;
using MediBook.RecordService.Entities;
using MediBook.RecordService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediBook.RecordService.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        private readonly RecordDbContext _context;

        public RecordRepository(RecordDbContext context)
        {
            _context = context;
        }

        public async Task<MedicalRecord?> FindByRecordId(string recordId)
        {
            return await _context.MedicalRecords.FirstOrDefaultAsync(r => r.RecordId == recordId);
        }

        public async Task<MedicalRecord?> FindByAppointmentId(string appointmentId)
        {
            return await _context.MedicalRecords.FirstOrDefaultAsync(r => r.AppointmentId == appointmentId);
        }

        public async Task<List<MedicalRecord>> FindByPatientId(string patientId)
        {
            return await _context.MedicalRecords
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<MedicalRecord>> FindByProviderId(string providerId)
        {
            return await _context.MedicalRecords
                .Where(r => r.ProviderId == providerId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<MedicalRecord>> FindByPatientIdOrderByCreatedAtDesc(string patientId)
        {
            return await _context.MedicalRecords
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<MedicalRecord>> FindByFollowUpDate(DateTime followUpDate)
        {
            return await _context.MedicalRecords
                .Where(r => r.FollowUpDate.HasValue && r.FollowUpDate.Value.Date == followUpDate.Date)
                .ToListAsync();
        }

        public async Task<int> CountByPatientId(string patientId)
        {
            return await _context.MedicalRecords
                .Where(r => r.PatientId == patientId)
                .CountAsync();
        }

        public async Task<MedicalRecord> CreateRecord(MedicalRecord record)
        {
            _context.MedicalRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<MedicalRecord> UpdateRecord(MedicalRecord record)
        {
            _context.MedicalRecords.Update(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<bool> DeleteRecord(string recordId)
        {
            var record = await _context.MedicalRecords.FindAsync(recordId);
            if (record == null) return false;

            _context.MedicalRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<MedicalRecord>> GetAllRecords()
        {
            return await _context.MedicalRecords
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}
