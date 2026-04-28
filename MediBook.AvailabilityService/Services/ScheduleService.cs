using MediBook.AvailabilityService.DTOs;
using MediBook.AvailabilityService.Entities;
using MediBook.AvailabilityService.Interfaces;

namespace MediBook.AvailabilityService.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly ISlotRepository _repository;

        public ScheduleService(ISlotRepository repository)
        {
            _repository = repository;
        }

        public async Task<SlotResponse> AddSlot(AddSlotRequest request)
        {
            var slot = new AvailabilitySlot
            {
                SlotId = Guid.NewGuid().ToString(),
                ProviderId = request.ProviderId,
                Date = request.Date,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                DurationMinutes = request.DurationMinutes,
                IsBooked = false,
                IsBlocked = false,
                Recurrence = null,
                CreatedAt = DateTime.UtcNow
            };

            var createdSlot = await _repository.CreateSlot(slot);
            return MapToResponse(createdSlot);
        }

        public async Task<List<SlotResponse>> AddBulkSlots(AddBulkSlotsRequest request)
        {
            var slots = new List<AvailabilitySlot>();
            var currentDate = request.StartDate;

            while (currentDate <= request.EndDate)
            {
                var slot = new AvailabilitySlot
                {
                    SlotId = Guid.NewGuid().ToString(),
                    ProviderId = request.ProviderId,
                    Date = currentDate,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    DurationMinutes = request.DurationMinutes,
                    IsBooked = false,
                    IsBlocked = false,
                    Recurrence = "Bulk",
                    CreatedAt = DateTime.UtcNow
                };
                slots.Add(slot);
                currentDate = currentDate.AddDays(1);
            }

            var createdSlots = await _repository.CreateSlots(slots);
            return createdSlots.Select(MapToResponse).ToList();
        }

        public async Task<List<SlotResponse>> GenerateRecurringSlots(GenerateRecurringSlotsRequest request)
        {
            var slots = new List<AvailabilitySlot>();
            var currentDate = request.StartDate;

            while (currentDate <= request.EndDate)
            {
                var slot = new AvailabilitySlot
                {
                    SlotId = Guid.NewGuid().ToString(),
                    ProviderId = request.ProviderId,
                    Date = currentDate,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    DurationMinutes = request.DurationMinutes,
                    IsBooked = false,
                    IsBlocked = false,
                    Recurrence = request.RecurrencePattern,
                    CreatedAt = DateTime.UtcNow
                };
                slots.Add(slot);

                // Advance based on recurrence pattern
                if (request.RecurrencePattern == "Daily")
                {
                    currentDate = currentDate.AddDays(1);
                }
                else if (request.RecurrencePattern == "Weekly")
                {
                    currentDate = currentDate.AddDays(7);
                }
                else
                {
                    currentDate = currentDate.AddDays(1);
                }
            }

            var createdSlots = await _repository.CreateSlots(slots);
            return createdSlots.Select(MapToResponse).ToList();
        }

        public async Task<List<SlotResponse>> GetSlotsByProvider(string providerId)
        {
            var slots = await _repository.FindByProviderId(providerId);
            return slots.Select(MapToResponse).ToList();
        }

        public async Task<List<SlotResponse>> GetAvailableSlots(string providerId, DateTime date)
        {
            var slots = await _repository.FindAvailableByProviderAndDate(providerId, date);
            return slots.Select(MapToResponse).ToList();
        }

        public async Task<SlotResponse?> GetSlotById(string slotId)
        {
            var slot = await _repository.FindBySlotId(slotId);
            return slot == null ? null : MapToResponse(slot);
        }

        public async Task<SlotResponse?> BookSlot(string slotId)
        {
            var slot = await _repository.FindBySlotId(slotId);
            if (slot == null || slot.IsBooked || slot.IsBlocked)
            {
                return null;
            }

            slot.IsBooked = true;
            var updatedSlot = await _repository.UpdateSlot(slot);
            return MapToResponse(updatedSlot);
        }

        public async Task<SlotResponse?> UnblockSlot(string slotId)
        {
            var slot = await _repository.FindBySlotId(slotId);
            if (slot == null) return null;

            slot.IsBlocked = false;
            var updatedSlot = await _repository.UpdateSlot(slot);
            return MapToResponse(updatedSlot);
        }

        public async Task<SlotResponse?> BlockSlot(string slotId, BlockSlotRequest request)
        {
            var slot = await _repository.FindBySlotId(slotId);
            if (slot == null) return null;

            slot.IsBlocked = request.IsBlocked;
            var updatedSlot = await _repository.UpdateSlot(slot);
            return MapToResponse(updatedSlot);
        }

        public async Task<SlotResponse?> UpdateSlot(string slotId, UpdateSlotRequest request)
        {
            var slot = await _repository.FindBySlotId(slotId);
            if (slot == null) return null;

            slot.Date = request.Date ?? slot.Date;
            slot.StartTime = request.StartTime ?? slot.StartTime;
            slot.EndTime = request.EndTime ?? slot.EndTime;
            slot.DurationMinutes = request.DurationMinutes ?? slot.DurationMinutes;

            var updatedSlot = await _repository.UpdateSlot(slot);
            return MapToResponse(updatedSlot);
        }

        public async Task<bool> DeleteSlot(string slotId)
        {
            return await _repository.DeleteSlot(slotId);
        }

        private SlotResponse MapToResponse(AvailabilitySlot slot)
        {
            return new SlotResponse
            {
                SlotId = slot.SlotId,
                ProviderId = slot.ProviderId,
                Date = slot.Date,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                DurationMinutes = slot.DurationMinutes,
                IsBooked = slot.IsBooked,
                IsBlocked = slot.IsBlocked,
                Recurrence = slot.Recurrence,
                CreatedAt = slot.CreatedAt
            };
        }
    }
}
