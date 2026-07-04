using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetAllAsync();
        public Task<IEnumerable<Category>> GetAllByProductIdAsync(Guid productId);
        public Task<Category?> GetByIdAsync(Guid id);
        public void Add(Category category);
        public void Remove(Category category);
    }
}
