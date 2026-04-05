using E_commerce_API.src.Domain.Entities;

namespace E_commerce_API.src.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> GetAll();
        public Task<Order?> GetById(int id);
        public void Add(Order order);
        public void Remove(Order order);
    }
}
