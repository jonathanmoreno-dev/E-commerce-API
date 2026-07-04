using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        public Task<IEnumerable<Cart>> GetAllAsync();
        public Task<Cart?> GetByIdAsync(Guid id);
        public Task<Cart?> GetByUserIdAsync(Guid userId);
        public void Add(Cart cart);
        public void Remove(Cart cart);
    }
}
