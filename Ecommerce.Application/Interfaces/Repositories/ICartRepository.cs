using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        public Task<PagedList<Cart>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        public void Add(Cart cart);
        public void Remove(Cart cart);
    }
}
