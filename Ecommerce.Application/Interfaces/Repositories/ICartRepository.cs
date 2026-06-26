using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        public Task<IEnumerable<Cart>> GetAllAsync();
        public Task<Cart?> GetByIdAsync(int id);
        public Task<Cart?> GetByUserIdAsync(int userId);
        public void Add(Cart cart);
        public void Update(Cart cart);
        public void Remove(Cart cart);
    }
}
