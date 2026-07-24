using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Pagination;
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
        public async Task<PagedList<Checkout>> GetAllActiveAsync(PaginationParams paginationParams)
        {
            var query = _appDbContext.Checkouts.Include(x => x.CheckoutItems).ThenInclude(y => y.Product)
                .Where(x => x.ExpiresAt > DateTime.UtcNow && x.PaymentAttempts.Any()).AsNoTracking();
            var totalItems = await query.CountAsync();
            var checkouts = await query.OrderByDescending(x => x.CreatedAt).Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync();

            return new PagedList<Checkout>(checkouts, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<PagedList<Checkout>> GetAllActiveByUserIdAsync(Guid userId, PaginationParams paginationParams)
        {
            var query = _appDbContext.Checkouts.Include(x => x.CheckoutItems).ThenInclude(y => y.Product)
                .Where(x => x.UserId == userId).Where(x => x.ExpiresAt > DateTime.UtcNow && x.PaymentAttempts.Any()).AsNoTracking();
            var totalItems = await query.CountAsync();
            var checkouts = await query.OrderByDescending(x => x.CreatedAt).Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync();

            return new PagedList<Checkout>(checkouts, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<Checkout?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Checkouts.Include(x => x.CheckoutItems).ThenInclude(y => y.Product).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Checkout?> GetByIdWithPaymentAttemptsAsync(Guid id)
        {
            return await _appDbContext.Checkouts.Include(x => x.PaymentAttempts).Include(x => x.CheckoutItems).ThenInclude(y => y.Product).FirstOrDefaultAsync(x => x.Id == id);
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
