using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface ICheckoutRepository
    {
        public Task<PagedList<Checkout>> GetAllActiveWithPaymentAttemptsAsync(PaginationParams paginationParams);
        public Task<PagedList<Checkout>> GetAllActiveWithPaymentAttemptsByUserIdAsync(Guid userId, PaginationParams paginationParams);
        public Task<IEnumerable<Checkout>> GetAllExpiredNotProcessedAsync();
        public Task<Checkout?> GetByIdAsync(Guid id);
        public Task<Checkout?> GetByIdWithPaymentAttemptsAsync(Guid id);
        public void Add(Checkout checkout);
        public void Remove(Checkout checkout);
    }
}
