using Ecommerce.Application.Interfaces.Repositories;
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
        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _appDbContext.Carts.Include(x => x.User).AsNoTracking().ToListAsync();
        }
        public async Task<Cart?> GetByIdAsync(int id)
        {
            return await _appDbContext.Carts.Include(x => x.CartItems).ThenInclude(y => y.Product).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Cart?> GetByUserIdAsync(int userId)
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
