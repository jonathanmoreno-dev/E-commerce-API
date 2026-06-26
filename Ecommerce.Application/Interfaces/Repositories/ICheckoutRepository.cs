using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface ICheckoutRepository
    {
        public Task<IEnumerable<Checkout>> GetAllAsync();
        public Task<IEnumerable<Checkout>> GetAllByUserIdAsync(int userId);
        public Task<Checkout?> GetByIdAsync(int id);
        public void Add(Checkout checkout);
        public void Update(Checkout checkout);
        public void Remove(Checkout checkout);
    }
}
