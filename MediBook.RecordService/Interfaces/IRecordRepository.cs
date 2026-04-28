using MediBook.RecordService.Entities;

namespace MediBook.RecordService.Interfaces
{
    public interface IRecordRepository
    {
        Task<MedicalRecord?> FindByRecordId(string recordId);
        Task<MedicalRecord?> FindByAppointmentId(string appointmentId);
        Task<List<MedicalRecord>> FindByPatientId(string patientId);
        Task<List<MedicalRecord>> FindByProviderId(string providerId);
        Task<List<MedicalRecord>> FindByPatientIdOrderByCreatedAtDesc(string patientId);
        Task<List<MedicalRecord>> FindByFollowUpDate(DateTime followUpDate);
        Task<int> CountByPatientId(string patientId);
        Task<MedicalRecord> CreateRecord(MedicalRecord record);
        Task<MedicalRecord> UpdateRecord(MedicalRecord record);
        Task<bool> DeleteRecord(string recordId);
        Task<List<MedicalRecord>> GetAllRecords();
    }
}
