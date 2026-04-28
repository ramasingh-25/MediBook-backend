using MediBook.AvailabilityService.Entities;

namespace MediBook.AvailabilityService.Interfaces
{
    public interface ISlotRepository
    {
        Task<AvailabilitySlot?> FindBySlotId(string slotId);
        Task<List<AvailabilitySlot>> FindByProviderId(string providerId);
        Task<List<AvailabilitySlot>> FindByProviderIdAndDate(string providerId, DateTime date);
        Task<List<AvailabilitySlot>> FindAvailableByProviderAndDate(string providerId, DateTime date);
        Task<List<AvailabilitySlot>> FindByDateBetween(DateTime startDate, DateTime endDate);
        Task<int> CountAvailableByProviderId(string providerId);
        Task<AvailabilitySlot> CreateSlot(AvailabilitySlot slot);
        Task<AvailabilitySlot> UpdateSlot(AvailabilitySlot slot);
        Task<bool> DeleteSlot(string slotId);
        Task<List<AvailabilitySlot>> CreateSlots(List<AvailabilitySlot> slots);
    }
}
