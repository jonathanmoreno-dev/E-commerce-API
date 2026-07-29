using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        public Task<PagedList<Order>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<PagedList<Order>> GetAllByUserIdAsync(Guid userId, PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<PagedList<Order>> GetAllByUserIdAndStatusAsync(Guid userId, OrderStatus status, PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public void Add(Order order);
    }
}
