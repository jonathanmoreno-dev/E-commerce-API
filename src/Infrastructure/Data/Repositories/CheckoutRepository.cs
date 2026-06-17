using E_commerce_API.src.Application.Interfaces.Repositories;
using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_API.src.Infrastructure.Data.Repositories
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly AppDbContext _appDbContext;
        public CheckoutRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IEnumerable<Checkout>> GetAllAsync()
        {
            return await _appDbContext.Checkouts.AsNoTracking().ToListAsync();
        }
        public async Task<Checkout?> GetByIdAsync(int id)
        {
            return await _appDbContext.Checkouts.FirstOrDefaultAsync(x => x.Id == id);
        }
        public void Add(Checkout checkout)
        {
            _appDbContext.Checkouts.Add(checkout);
        }
        public void Update(Checkout checkout)
        {
            _appDbContext.Checkouts.Update(checkout);
        }
        public void Remove(Checkout checkout)
        {
            _appDbContext.Checkouts.Remove(checkout);
        }
    }
}
