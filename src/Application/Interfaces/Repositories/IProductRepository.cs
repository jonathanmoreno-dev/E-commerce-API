using E_commerce_API.src.Domain.Entities;

namespace E_commerce_API.src.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> GetAllAsync();
        public Task<Product?> GetByIdAsync(int id);
        public void Add(Product product);
        public void Update(Product product);
        public void Remove(Product product);
    }
}
