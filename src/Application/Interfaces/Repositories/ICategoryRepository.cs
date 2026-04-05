using E_commerce_API.src.Domain.Entities;

namespace E_commerce_API.src.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetAll();
        public Task<Category?> GetById(int id);
        public void Add(Category category);
        public void Remove(Category category);
    }
}
