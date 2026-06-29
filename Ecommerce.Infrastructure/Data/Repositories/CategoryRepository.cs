using System.Xml.Schema;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Repositories
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
        public async Task<IEnumerable<Category>> GetAllByProductIdAsync(int productId)
        {
            return await _appDbContext.Categories.Where(x => x.Products.Any(x => x.Id == productId)).AsNoTracking().ToListAsync();
        }
        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _appDbContext.Categories.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);
        }
        public void Add(Category category)
        {
            _appDbContext.Categories.Add(category);
        }
        public void Remove(Category category)
        {
            _appDbContext.Categories.Remove(category);
        }
    }
}
