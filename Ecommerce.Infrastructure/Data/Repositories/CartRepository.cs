using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _appDbContext;
        public CartRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<PagedList<Cart>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var query = _appDbContext.Carts.Include(x => x.User).AsNoTracking();
            var totalItems = await query.CountAsync(cancellationToken);
            var carts = await query.OrderByDescending(x => x.CreatedAt).Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync(cancellationToken);

            return new PagedList<Cart>(carts, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _appDbContext.Carts.Include(x => x.CartItems).ThenInclude(y => y.Product).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _appDbContext.Carts.Include(x => x.CartItems).ThenInclude(y => y.Product).FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        }
        public void Add(Cart cart)
        {
            _appDbContext.Carts.Add(cart);
        }
        public void Remove(Cart cart)
        {
            _appDbContext.Carts.Remove(cart);
        }
    }
}
