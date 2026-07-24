using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        public Task<PagedList<Order>> GetAllAsync(PaginationParams paginationParams);
        public Task<PagedList<Order>> GetAllByUserIdAsync(Guid userId, PaginationParams paginationParams);
        public Task<PagedList<Order>> GetAllByUserIdAndStatusAsync(Guid userId, OrderStatus status, PaginationParams paginationParams);
        public Task<Order?> GetByIdAsync(Guid id);
        public Task<Order?> GetByIdForDetailsAsync(Guid id);
        public void Add(Order order);
    }
}
