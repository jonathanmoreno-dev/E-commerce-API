using E_commerce_API.src.Application.Interfaces.Repositories;
using E_commerce_API.src.Domain.Entities;
using E_commerce_API.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_API.src.Infrastructure.Data.Repositories
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
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
        public void Add(User user)
        {
            _appDbContext.Users.Add(user);
        }
        public void Update(User user)
        {
            _appDbContext.Users.Update(user);
        }
        public void Remove(User user)
        {
            _appDbContext.Users.Remove(user);
        }
    }
}
