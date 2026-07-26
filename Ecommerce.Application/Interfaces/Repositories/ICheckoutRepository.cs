using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface ICheckoutRepository
    {
        public Task<PagedList<Checkout>> GetAllActiveWithPaymentAttemptsAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<PagedList<Checkout>> GetAllActiveWithPaymentAttemptsByUserIdAsync(Guid userId, PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<IEnumerable<Checkout>> GetAllExpiredNotProcessedAsync(CancellationToken cancellationToken);
        public Task<Checkout?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<Checkout?> GetByIdWithPaymentAttemptsAsync(Guid id, CancellationToken cancellationToken);
        public void Add(Checkout checkout);
        public void Remove(Checkout checkout);
    }
}
