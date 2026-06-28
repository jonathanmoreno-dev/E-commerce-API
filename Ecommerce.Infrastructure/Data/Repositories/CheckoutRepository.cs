using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Repositories
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly AppDbContext _appDbContext;
        public CheckoutRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IEnumerable<Checkout>> GetAllActiveAsync()
        {
            return await _appDbContext.Checkouts.Where(x => x.IsActive).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<Checkout>> GetAllActiveByUserIdAsync(int userId)
        {
            return await _appDbContext.Checkouts.Where(x => x.UserId == userId).Where(x => x.IsActive).AsNoTracking().ToListAsync();
        }
        public async Task<Checkout?> GetByIdAsync(int id)
        {
            return await _appDbContext.Checkouts.FirstOrDefaultAsync(x => x.Id == id);
        }
        public void Add(Checkout checkout)
        {
            _appDbContext.Checkouts.Add(checkout);
        }
        public void Remove(Checkout checkout)
        {
            _appDbContext.Checkouts.Remove(checkout);
        }
    }
}
