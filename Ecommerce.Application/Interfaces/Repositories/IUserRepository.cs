using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllAsync();
        public Task<IEnumerable<User>> GetAllAdminsAsync();
        public Task<IEnumerable<User>> GetAllStandardUsersAsync();
        public Task<User?> GetByIdAsync(Guid id);
        public Task<User?> GetByEmailAsync(string email);
        public void Add(User user);
        public void Remove(User user);
    }
}
