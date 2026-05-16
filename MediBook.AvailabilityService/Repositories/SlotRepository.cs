using MediBook.AvailabilityService.Data;
using MediBook.AvailabilityService.Entities;
using MediBook.AvailabilityService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediBook.AvailabilityService.Repositories
{
    public class SlotRepository : ISlotRepository
    {
        private readonly SlotDbContext _context;

        public SlotRepository(SlotDbContext context)
        {
            _context = context;
        }

        public async Task<AvailabilitySlot?> FindBySlotId(string slotId)
        {
            return await _context.AvailabilitySlots.FirstOrDefaultAsync(s => s.SlotId == slotId);
        }

        public async Task<List<AvailabilitySlot>> FindByProviderId(string providerId)
        {
            return await _context.AvailabilitySlots
                .Where(s => s.ProviderId.ToLower() == providerId.ToLower())
                .OrderBy(s => s.Date)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<List<AvailabilitySlot>> FindByProviderIdAndDate(string providerId, DateTime date)
        {
            var targetDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
            return await _context.AvailabilitySlots
                .Where(s => s.ProviderId == providerId && s.Date >= targetDate && s.Date < targetDate.AddDays(1))
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<List<AvailabilitySlot>> FindAvailableByProviderAndDate(string providerId, DateTime date)
        {
            var targetDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
            return await _context.AvailabilitySlots
                .Where(s => s.ProviderId.ToLower() == providerId.ToLower() && 
                           s.Date >= targetDate && s.Date < targetDate.AddDays(1) && 
                           !s.IsBooked && 
                           !s.IsBlocked)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<List<AvailabilitySlot>> FindByDateBetween(DateTime startDate, DateTime endDate)
        {
            return await _context.AvailabilitySlots
                .Where(s => s.Date >= startDate && s.Date <= endDate)
                .OrderBy(s => s.Date)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<int> CountAvailableByProviderId(string providerId)
        {
            return await _context.AvailabilitySlots
                .CountAsync(s => s.ProviderId == providerId && !s.IsBooked && !s.IsBlocked);
        }

        public async Task<AvailabilitySlot> CreateSlot(AvailabilitySlot slot)
        {
            _context.AvailabilitySlots.Add(slot);
            await _context.SaveChangesAsync();
            return slot;
        }

        public async Task<AvailabilitySlot> UpdateSlot(AvailabilitySlot slot)
        {
            _context.AvailabilitySlots.Update(slot);
            await _context.SaveChangesAsync();
            return slot;
        }

        public async Task<bool> DeleteSlot(string slotId)
        {
            var slot = await _context.AvailabilitySlots.FindAsync(slotId);
            if (slot == null) return false;

            _context.AvailabilitySlots.Remove(slot);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<AvailabilitySlot>> CreateSlots(List<AvailabilitySlot> slots)
        {
            await _context.AvailabilitySlots.AddRangeAsync(slots);
            await _context.SaveChangesAsync();
            return slots;
        }
    }
}
