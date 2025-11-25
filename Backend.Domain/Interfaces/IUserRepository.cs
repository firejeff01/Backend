using Backend.Domain.Entities;
using System.Threading.Tasks;

namespace Backend.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
    }
}