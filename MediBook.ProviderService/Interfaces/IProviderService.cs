using MediBook.ProviderService.DTOs;

namespace MediBook.ProviderService.Interfaces
{
    public interface IProviderService
    {
        Task<ProviderResponse> RegisterProvider(RegisterProviderRequest request);
        Task<ProviderResponse?> GetProviderById(string providerId);
        Task<List<ProviderResponse>> GetBySpecialization(string specialization);
        Task<List<ProviderResponse>> SearchProviders(string searchTerm);
        Task<ProviderResponse?> UpdateProvider(string providerId, UpdateProviderRequest request);
        Task<ProviderResponse?> VerifyProvider(string providerId);
        Task<ProviderResponse?> SetAvailability(string providerId, SetAvailabilityRequest request);
        Task<bool> DeleteProvider(string providerId);
        Task<ProviderResponse?> UpdateRating(string providerId, double newRating);
        Task<List<ProviderResponse>> GetAllProviders();
    }
}
