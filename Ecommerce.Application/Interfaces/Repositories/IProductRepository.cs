using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        public Task<PagedList<Product>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<PagedList<Product>> GetAllByCategoryIdAsync(Guid categoryId, PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public void Add(Product product);
        public void Remove(Product product);
    }
}
