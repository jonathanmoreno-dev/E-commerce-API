using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;
        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _appDbContext.Products.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetAllByCategoryIdAsync(Guid categoryId)
        {
            return await _appDbContext.Products.Where(x => x.Categories.Any(x => x.Id == categoryId)).AsNoTracking().ToListAsync();
        }
        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Products.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }
        public void Add(Product product)
        {
            _appDbContext.Products.Add(product);
        }
        public void Remove(Product product)
        {
            _appDbContext.Products.Remove(product);
        }
    }
}
