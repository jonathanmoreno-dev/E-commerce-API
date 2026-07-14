using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _appDbContext.Users.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<User>> GetAllByRoleAsync(UserRole role)
        {
            return await _appDbContext.Users.Where(x => x.Role == role).AsNoTracking().ToListAsync();
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(x => x.Email.Value == email);
        }
        public void Add(User user)
        {
            _appDbContext.Users.Add(user);
        }
        public void Remove(User user)
        {
            _appDbContext.Users.Remove(user);
        }
    }
}
