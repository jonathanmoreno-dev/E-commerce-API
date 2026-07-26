using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        public Task<PagedList<Category>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<PagedList<Category>> GetAllByProductIdAsync(Guid productId, PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public void Add(Category category);
        public void Remove(Category category);
    }
}
