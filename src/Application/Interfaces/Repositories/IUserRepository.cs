using E_commerce_API.src.Domain.Entities;

namespace E_commerce_API.src.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllAsync();
        public Task<User?> GetByIdAsync(int id);
        public void Add(User user);
        public void Update(User user);
        public void Remove(User user);
    }
}
