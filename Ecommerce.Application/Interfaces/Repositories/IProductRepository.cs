using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        public Task<PagedList<Product>> GetAllAsync(PaginationParams paginationParams);
        public Task<PagedList<Product>> GetAllByCategoryIdAsync(Guid categoryId, PaginationParams paginationParams);
        public Task<Product?> GetByIdAsync(Guid id);
        public void Add(Product product);
        public void Remove(Product product);
    }
}
