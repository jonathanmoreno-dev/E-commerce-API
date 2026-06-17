using E_commerce_API.src.Domain.Entities;

namespace E_commerce_API.src.Application.Interfaces.Repositories
{
    public interface ICheckoutRepository
    {
        public Task<IEnumerable<Checkout>> GetAllAsync();
        public Task<Checkout> GetByIdAsync(int id);
        public void Add(Checkout checkout);
        public void Update(Checkout checkout);
        public void Remove(Checkout checkout);
    }
}
