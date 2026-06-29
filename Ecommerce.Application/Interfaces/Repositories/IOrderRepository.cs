using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> GetAllAsync();
        public Task<IEnumerable<Order>> GetAllByUserIdAsync(int userId);
        public Task<IEnumerable<Order>> GetAllByUserIdAndStatusAsync(int userId, OrderStatus status);
        public Task<Order?> GetByIdAsync(int id);
        public void Add(Order order);
    }
}
