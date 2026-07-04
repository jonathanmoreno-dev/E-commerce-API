using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> GetAllAsync();
        public Task<IEnumerable<Product>> GetAllByCategoryIdAsync(Guid categoryId);
        public Task<Product?> GetByIdAsync(Guid id);
        public void Add(Product product);
        public void Remove(Product product);
    }
}
