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
        public async Task<PagedList<Cart>> GetAllAsync(PaginationParams paginationParams)
        {
            var query = _appDbContext.Carts.Include(x => x.User).AsNoTracking();
            var totalItems = await query.CountAsync();
            var carts = await query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync();

            return new PagedList<Cart>(carts, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<Cart?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Carts.Include(x => x.CartItems).ThenInclude(y => y.Product).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Cart?> GetByUserIdAsync(Guid userId)
        {
            return await _appDbContext.Carts.Include(x => x.CartItems).ThenInclude(y => y.Product).FirstOrDefaultAsync(x => x.UserId == userId);
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
