using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface ICheckoutRepository
    {
        public Task<PagedList<Checkout>> GetAllActiveAsync(PaginationParams paginationParams);
        public Task<PagedList<Checkout>> GetAllActiveByUserIdAsync(Guid userId, PaginationParams paginationParams);
        public Task<Checkout?> GetByIdAsync(Guid id);
        public Task<Checkout?> GetByIdWithPaymentAttemptsAsync(Guid id);
        public void Add(Checkout checkout);
        public void Remove(Checkout checkout);
    }
}
