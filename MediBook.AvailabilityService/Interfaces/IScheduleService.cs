using MediBook.AvailabilityService.DTOs;

namespace MediBook.AvailabilityService.Interfaces
{
    public interface IScheduleService
    {
        Task<SlotResponse> AddSlot(AddSlotRequest request);
        Task<List<SlotResponse>> AddBulkSlots(AddBulkSlotsRequest request);
        Task<List<SlotResponse>> GenerateRecurringSlots(GenerateRecurringSlotsRequest request);
        Task<List<SlotResponse>> GetSlotsByProvider(string providerId);
        Task<List<SlotResponse>> GetAvailableSlots(string providerId, DateTime date);
        Task<SlotResponse?> GetSlotById(string slotId);
        Task<SlotResponse?> BookSlot(string slotId);
        Task<SlotResponse?> UnblockSlot(string slotId);
        Task<SlotResponse?> BlockSlot(string slotId, BlockSlotRequest request);
        Task<SlotResponse?> UpdateSlot(string slotId, UpdateSlotRequest request);
        Task<bool> DeleteSlot(string slotId);
    }
}
