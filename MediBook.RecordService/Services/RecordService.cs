using MediBook.RecordService.DTOs;
using MediBook.RecordService.Entities;
using MediBook.RecordService.Interfaces;

namespace MediBook.RecordService.Services
{
    public class RecordService : IRecordService
    {
        private readonly IRecordRepository _repository;

        public RecordService(IRecordRepository repository)
        {
            _repository = repository;
        }

        public async Task<RecordResponse> CreateRecord(CreateRecordRequest request)
        {
            var existingRecord = await _repository.FindByAppointmentId(request.AppointmentId);
            if (existingRecord != null)
            {
                // Intelligent Upsert: If it exists, update it instead of failing
                existingRecord.Diagnosis = request.Diagnosis;
                existingRecord.Prescription = request.Prescription;
                existingRecord.Notes = request.Notes;
                if (request.AttachmentUrl != null) existingRecord.AttachmentUrl = request.AttachmentUrl;
                if (request.FollowUpDate.HasValue) existingRecord.FollowUpDate = request.FollowUpDate;
                existingRecord.UpdatedAt = DateTime.UtcNow;

                var updated = await _repository.UpdateRecord(existingRecord);
                return MapToResponse(updated);
            }

            var record = new MedicalRecord
            {
                RecordId = Guid.NewGuid().ToString(),
                AppointmentId = request.AppointmentId,
                PatientId = request.PatientId,
                ProviderId = request.ProviderId,
                Diagnosis = request.Diagnosis,
                Prescription = request.Prescription,
                Notes = request.Notes,
                AttachmentUrl = request.AttachmentUrl,
                FollowUpDate = request.FollowUpDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdRecord = await _repository.CreateRecord(record);
            return MapToResponse(createdRecord);
        }

        public async Task<RecordResponse?> GetRecordByAppointment(string appointmentId)
        {
            var record = await _repository.FindByAppointmentId(appointmentId);
            return record == null ? null : MapToResponse(record);
        }

        public async Task<List<RecordResponse>> GetRecordsByPatient(string patientId)
        {
            var records = await _repository.FindByPatientId(patientId);
            return records.Select(MapToResponse).ToList();
        }

        public async Task<List<RecordResponse>> GetRecordsByProvider(string providerId)
        {
            var records = await _repository.FindByProviderId(providerId);
            return records.Select(MapToResponse).ToList();
        }

        public async Task<RecordResponse?> GetRecordById(string recordId)
        {
            var record = await _repository.FindByRecordId(recordId);
            return record == null ? null : MapToResponse(record);
        }

        public async Task<RecordResponse?> UpdateRecord(string recordId, UpdateRecordRequest request)
        {
            var record = await _repository.FindByRecordId(recordId);
            if (record == null) return null;

            if (request.Diagnosis != null)
                record.Diagnosis = request.Diagnosis;
            if (request.Prescription != null)
                record.Prescription = request.Prescription;
            if (request.Notes != null)
                record.Notes = request.Notes;
            if (request.AttachmentUrl != null)
                record.AttachmentUrl = request.AttachmentUrl;
            if (request.FollowUpDate.HasValue)
                record.FollowUpDate = request.FollowUpDate;

            record.UpdatedAt = DateTime.UtcNow;

            var updatedRecord = await _repository.UpdateRecord(record);
            return MapToResponse(updatedRecord);
        }

        public async Task<List<RecordResponse>> GetFollowUpRecords(DateTime followUpDate)
        {
            var records = await _repository.FindByFollowUpDate(followUpDate);
            return records.Select(MapToResponse).ToList();
        }

        public async Task<int> GetRecordCount(string patientId)
        {
            return await _repository.CountByPatientId(patientId);
        }

        public async Task<bool> DeleteRecord(string recordId)
        {
            return await _repository.DeleteRecord(recordId);
        }

        public async Task<List<RecordResponse>> GetAllRecords()
        {
            var records = await _repository.GetAllRecords();
            return records.Select(MapToResponse).ToList();
        }

        private RecordResponse MapToResponse(MedicalRecord record)
        {
            return new RecordResponse
            {
                RecordId = record.RecordId,
                AppointmentId = record.AppointmentId,
                PatientId = record.PatientId,
                ProviderId = record.ProviderId,
                Diagnosis = record.Diagnosis,
                Prescription = record.Prescription,
                Notes = record.Notes,
                AttachmentUrl = record.AttachmentUrl,
                FollowUpDate = record.FollowUpDate,
                CreatedAt = record.CreatedAt,
                UpdatedAt = record.UpdatedAt
            };
        }
    }
}
