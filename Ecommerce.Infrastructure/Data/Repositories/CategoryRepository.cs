using System.Xml.Schema;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Pagination;
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
        public async Task<PagedList<Category>> GetAllAsync(PaginationParams paginationParams)
        {
            var query = _appDbContext.Categories.AsNoTracking();
            var totalItems = await query.CountAsync();
            var categories = await query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync();

            return new PagedList<Category>(categories, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<PagedList<Category>> GetAllByProductIdAsync(Guid productId, PaginationParams paginationParams)
        {
            var query = _appDbContext.Categories.Where(x => x.Products.Any(x => x.Id == productId)).AsNoTracking();
            var totalItems = await query.CountAsync();
            var categories = await query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync();

            return new PagedList<Category>(categories, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<Category?> GetByIdAsync(Guid id)
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
