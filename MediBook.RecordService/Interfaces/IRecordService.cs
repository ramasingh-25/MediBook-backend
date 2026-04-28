using MediBook.RecordService.DTOs;

namespace MediBook.RecordService.Interfaces
{
    public interface IRecordService
    {
        Task<RecordResponse> CreateRecord(CreateRecordRequest request);
        Task<RecordResponse?> GetRecordByAppointment(string appointmentId);
        Task<List<RecordResponse>> GetRecordsByPatient(string patientId);
        Task<List<RecordResponse>> GetRecordsByProvider(string providerId);
        Task<RecordResponse?> GetRecordById(string recordId);
        Task<RecordResponse?> UpdateRecord(string recordId, UpdateRecordRequest request);
        Task<List<RecordResponse>> GetFollowUpRecords(DateTime followUpDate);
        Task<int> GetRecordCount(string patientId);
        Task<bool> DeleteRecord(string recordId);
        Task<List<RecordResponse>> GetAllRecords();
    }
}
