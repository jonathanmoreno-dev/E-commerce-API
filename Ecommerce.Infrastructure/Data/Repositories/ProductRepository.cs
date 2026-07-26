using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Pagination;
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
        public async Task<PagedList<Product>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var query = _appDbContext.Products.AsNoTracking();
            var totalItems = await query.CountAsync(cancellationToken);
            var products = await query.OrderBy(x => x.Name).Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync(cancellationToken);

            return new PagedList<Product>(products, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<PagedList<Product>> GetAllByCategoryIdAsync(Guid categoryId, PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var query = _appDbContext.Products.Where(x => x.Categories.Any(x => x.Id == categoryId)).AsNoTracking();
            var totalItems = await query.CountAsync(cancellationToken);
            var products = await query.OrderBy(x => x.Name).Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync(cancellationToken);

            return new PagedList<Product>(products, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _appDbContext.Products.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
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
