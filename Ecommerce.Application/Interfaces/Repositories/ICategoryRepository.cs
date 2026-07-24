using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        public Task<PagedList<Category>> GetAllAsync(PaginationParams paginationParams);
        public Task<PagedList<Category>> GetAllByProductIdAsync(Guid productId, PaginationParams paginationParams);
        public Task<Category?> GetByIdAsync(Guid id);
        public void Add(Category category);
        public void Remove(Category category);
    }
}
