using System.Collections.Generic;
using System.Threading.Tasks;
using MediBook.AuthService.Entities;

namespace MediBook.AuthService.Interfaces
{
    public interface IUserRepository
    {
        Task<User> FindByEmail(string email);
        Task<User> FindByUserId(string userId);
        Task<bool> ExistsByEmail(string email);
        Task<List<User>> FindAllByRole(string role);
        Task<User> FindByPhone(string phone);
        Task<List<User>> FindByFullNameContaining(string name);
        Task<bool> DeleteByUserId(string userId);
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user);
    }
}