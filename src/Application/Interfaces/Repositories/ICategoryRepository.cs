using E_commerce_API.src.Domain.Entities;

namespace E_commerce_API.src.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetAllAsync();
        public Task<Category?> GetByIdAsync(int id);
        public void Add(Category category);
        public void Update(Category category);
        public void Remove(Category category);
    }
}
