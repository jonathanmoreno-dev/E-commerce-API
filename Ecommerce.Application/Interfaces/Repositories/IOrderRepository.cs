using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> GetAllAsync();
        public Task<IEnumerable<Order>> GetAllByUserIdAsync(Guid userId);
        public Task<IEnumerable<Order>> GetAllByUserIdAndStatusAsync(Guid userId, OrderStatus status);
        public Task<Order?> GetByIdAsync(Guid id);
        public Task<Order?> GetByIdForDetailsAsync(Guid id);
        public void Add(Order order);
    }
}
