using E_commerce_API.src.Domain.Entities;

namespace E_commerce_API.src.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        public Task<IEnumerable<Cart>> GetAllAsync();
        public Task<Cart?> GetByIdAsync(int id);
        public void Add(Cart cart);
        public void Update(Cart cart);
        public void Remove(Cart cart);
    }
}
