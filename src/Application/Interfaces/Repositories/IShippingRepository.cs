using E_commerce_API.src.Domain.Entities;

namespace E_commerce_API.src.Application.Interfaces.Repositories
{
    public interface IShippingRepository
    {
        public Task<IEnumerable<Shipping>> GetAllAsync();
        public Task<Shipping?> GetByIdAsync(int id);
        public void Add(Shipping shipping);
        public void Update(Shipping shipping);
        public void Remove(Shipping shipping);
    }
}
