using MediBook.ProviderService.Entities;

namespace MediBook.ProviderService.Interfaces
{
    public interface IProviderRepository
    {
        Task<Provider?> FindByUserId(string userId);
        Task<Provider?> FindByProviderId(string providerId);
        Task<List<Provider>> FindBySpecialization(string specialization);
        Task<List<Provider>> FindByIsVerified(bool isVerified);
        Task<List<Provider>> FindByIsAvailable(bool isAvailable);
        Task<List<Provider>> SearchByNameOrSpecialization(string searchTerm);
        Task<List<Provider>> FindByClinicAddress(string address);
        Task<int> CountBySpecialization(string specialization);
        Task<Provider> CreateProvider(Provider provider);
        Task<Provider> UpdateProvider(Provider provider);
        Task<bool> DeleteProvider(string providerId);
        Task<List<Provider>> GetAllProviders();
    }
}
