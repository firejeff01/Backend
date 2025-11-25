using Backend.Domain.Interfaces;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BackendDbContext _db;
        public UserRepository(BackendDbContext db) { _db = db; }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var e = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (e == null) return null;
            return new User
            {
                Id = e.Id,
                Account = e.Account,
                Name = e.Name,
                Password = e.Password,
                Email = e.Email,
                Phone = e.Phone,
                CreateTime = e.CreateTime,
                UpdateTime = e.UpdateTime
            };
        }

        public async Task AddAsync(User user)
        {
            var e = new UserEntity
            {
                Account = user.Account,
                Name = user.Name,
                Password = user.Password,
                Email = user.Email,
                Phone = user.Phone,
                CreateTime = user.CreateTime,
                UpdateTime = user.UpdateTime
            };
            _db.Users.Add(e);
            await _db.SaveChangesAsync();
            user.Id = e.Id;
        }
    }
}