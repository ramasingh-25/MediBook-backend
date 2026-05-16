using MediBook.ProviderService.Data;
using MediBook.ProviderService.Entities;
using MediBook.ProviderService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediBook.ProviderService.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly ProviderDbContext _context;

        public ProviderRepository(ProviderDbContext context)
        {
            _context = context;
        }

        public async Task<Provider?> FindByUserId(string userId)
        {
            return await _context.Providers.FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<Provider?> FindByProviderId(string providerId)
        {
            return await _context.Providers.FirstOrDefaultAsync(p => p.ProviderId == providerId);
        }

        public async Task<List<Provider>> FindBySpecialization(string specialization)
        {
            return await _context.Providers
                .Where(p => p.Specialization == specialization && p.IsVerified)
                .ToListAsync();
        }

        public async Task<List<Provider>> FindByIsVerified(bool isVerified)
        {
            return await _context.Providers
                .Where(p => p.IsVerified == isVerified)
                .ToListAsync();
        }

        public async Task<List<Provider>> FindByIsAvailable(bool isAvailable)
        {
            return await _context.Providers
                .Where(p => p.IsAvailable == isAvailable && p.IsVerified)
                .ToListAsync();
        }

        public async Task<List<Provider>> SearchByNameOrSpecialization(string searchTerm)
        {
            return await _context.Providers
                .Where(p => p.IsVerified &&
                    (p.Specialization.Contains(searchTerm) ||
                     p.ClinicName.Contains(searchTerm) ||
                     p.ClinicAddress.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<List<Provider>> FindByClinicAddress(string address)
        {
            return await _context.Providers
                .Where(p => p.ClinicAddress.Contains(address) && p.IsVerified)
                .ToListAsync();
        }

        public async Task<int> CountBySpecialization(string specialization)
        {
            return await _context.Providers
                .CountAsync(p => p.Specialization == specialization && p.IsVerified);
        }

        public async Task<Provider> CreateProvider(Provider provider)
        {
            _context.Providers.Add(provider);
            await _context.SaveChangesAsync();
            return provider;
        }

        public async Task<Provider> UpdateProvider(Provider provider)
        {
            _context.Providers.Update(provider);
            await _context.SaveChangesAsync();
            return provider;
        }

        public async Task<bool> DeleteProvider(string providerId)
        {
            var provider = await _context.Providers.FindAsync(providerId);
            if (provider == null) return false;

            _context.Providers.Remove(provider);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Provider>> GetAllProviders()
        {
            return await _context.Providers.ToListAsync();
        }
    }
}
