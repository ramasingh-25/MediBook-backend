using MediBook.ProviderService.DTOs;
using MediBook.ProviderService.Entities;
using MediBook.ProviderService.Interfaces;

namespace MediBook.ProviderService.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _repository;

        public ProviderService(IProviderRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProviderResponse> RegisterProvider(RegisterProviderRequest request)
        {
            // Check if provider already exists for this user
            var existingProvider = await _repository.FindByUserId(request.UserId);
            if (existingProvider != null)
            {
                throw new InvalidOperationException("Provider profile already exists for this user.");
            }

            var provider = new Provider
            {
                ProviderId = Guid.NewGuid().ToString(),
                UserId = request.UserId,
                Specialization = request.Specialization,
                Qualification = request.Qualification,
                ExperienceYears = request.ExperienceYears,
                Bio = request.Bio,
                ClinicName = request.ClinicName,
                ClinicAddress = request.ClinicAddress,
                AvgRating = 0.0,
                IsVerified = false,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdProvider = await _repository.CreateProvider(provider);
            return MapToResponse(createdProvider);
        }

        public async Task<ProviderResponse?> GetProviderById(string providerId)
        {
            var provider = await _repository.FindByProviderId(providerId);
            return provider == null ? null : MapToResponse(provider);
        }

        public async Task<List<ProviderResponse>> GetBySpecialization(string specialization)
        {
            var providers = await _repository.FindBySpecialization(specialization);
            return providers.Select(MapToResponse).ToList();
        }

        public async Task<List<ProviderResponse>> SearchProviders(string searchTerm)
        {
            var providers = await _repository.SearchByNameOrSpecialization(searchTerm);
            return providers.Select(MapToResponse).ToList();
        }

        public async Task<ProviderResponse?> UpdateProvider(string providerId, UpdateProviderRequest request)
        {
            var provider = await _repository.FindByProviderId(providerId);
            if (provider == null) return null;

            provider.Specialization = request.Specialization ?? provider.Specialization;
            provider.Qualification = request.Qualification ?? provider.Qualification;
            provider.ExperienceYears = request.ExperienceYears ?? provider.ExperienceYears;
            provider.Bio = request.Bio ?? provider.Bio;
            provider.ClinicName = request.ClinicName ?? provider.ClinicName;
            provider.ClinicAddress = request.ClinicAddress ?? provider.ClinicAddress;
            provider.IsAvailable = request.IsAvailable ?? provider.IsAvailable;

            var updatedProvider = await _repository.UpdateProvider(provider);
            return MapToResponse(updatedProvider);
        }

        public async Task<ProviderResponse?> VerifyProvider(string providerId)
        {
            var provider = await _repository.FindByProviderId(providerId);
            if (provider == null) return null;

            provider.IsVerified = true;
            var updatedProvider = await _repository.UpdateProvider(provider);
            return MapToResponse(updatedProvider);
        }

        public async Task<ProviderResponse?> SetAvailability(string providerId, SetAvailabilityRequest request)
        {
            var provider = await _repository.FindByProviderId(providerId);
            if (provider == null) return null;

            provider.IsAvailable = request.IsAvailable;
            var updatedProvider = await _repository.UpdateProvider(provider);
            return MapToResponse(updatedProvider);
        }

        public async Task<bool> DeleteProvider(string providerId)
        {
            return await _repository.DeleteProvider(providerId);
        }

        public async Task<ProviderResponse?> UpdateRating(string providerId, double newRating)
        {
            var provider = await _repository.FindByProviderId(providerId);
            if (provider == null) return null;

            // Calculate new average rating (simple average for now)
            provider.AvgRating = (provider.AvgRating + newRating) / 2;
            var updatedProvider = await _repository.UpdateProvider(provider);
            return MapToResponse(updatedProvider);
        }

        public async Task<List<ProviderResponse>> GetAllProviders()
        {
            var providers = await _repository.GetAllProviders();
            return providers.Select(MapToResponse).ToList();
        }

        private ProviderResponse MapToResponse(Provider provider)
        {
            return new ProviderResponse
            {
                ProviderId = provider.ProviderId,
                UserId = provider.UserId,
                Specialization = provider.Specialization,
                Qualification = provider.Qualification,
                ExperienceYears = provider.ExperienceYears,
                Bio = provider.Bio,
                ClinicName = provider.ClinicName,
                ClinicAddress = provider.ClinicAddress,
                AvgRating = provider.AvgRating,
                IsVerified = provider.IsVerified,
                IsAvailable = provider.IsAvailable,
                CreatedAt = provider.CreatedAt
            };
        }
    }
}
