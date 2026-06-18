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
            return await _appDbContext.Carts.AsNoTracking().ToListAsync();
        }
        public async Task<Cart?> GetByIdAsync(int id)
        {
            return await _appDbContext.Carts.FirstOrDefaultAsync(x => x.Id == id);
        }
        public void Add(Cart cart)
        {
            _appDbContext.Carts.Add(cart);
        }
        public void Update(Cart cart)
        {
            _appDbContext.Carts.Update(cart);
        }
        public void Remove(Cart cart)
        {
            _appDbContext.Carts.Remove(cart);
        }
    }
}
