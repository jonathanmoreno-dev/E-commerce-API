using E_commerce_API.src.Application.Interfaces.Repositories;
using E_commerce_API.src.Domain.Entities;
using E_commerce_API.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_API.src.Infrastructure.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;
        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _appDbContext.Categories.AsNoTracking().ToListAsync();
        }
        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _appDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }
        public void Add(Category category)
        {
            _appDbContext.Categories.Add(category);
        }
        public void Update(Category category)
        {
            _appDbContext.Categories.Update(category);
        }
        public void Remove(Category category)
        {
            _appDbContext.Categories.Remove(category);
        }
    }
}
