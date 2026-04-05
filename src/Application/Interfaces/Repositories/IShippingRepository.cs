using E_commerce_API.src.Domain.Entities;

namespace E_commerce_API.src.Application.Interfaces.Repositories
{
    public interface IShippingRepository
    {
        public Task<IEnumerable<Shipping>> GetAll();
        public Task<Shipping?> GetById(int id);
        public void Add(Shipping shipping);
        public void Remove(Shipping shipping);
    }
}
