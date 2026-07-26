using Ecommerce.Application.DTOs.CheckoutDTOs;
using Ecommerce.Application.DTOs.CheckoutItemDTOs;
using Ecommerce.Application.Pagination;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICheckoutService
    {
        public Task<PagedList<CheckoutSummaryDTO>> GetAllActiveAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<PagedList<CheckoutSummaryDTO>> GetAllActiveByUserIdAsync(Guid userId, PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<PagedList<CheckoutSummaryDTO>> GetAllCurrentUserCheckoutsActiveAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<CheckoutDetailsDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<CheckoutDetailsDTO> CreateAsync(CancellationToken cancellationToken);
        public Task<CheckoutDetailsDTO> UpdateAsync(Guid checkoutId, CheckoutUpdateDTO checkoutUpdate, CancellationToken cancellationToken);
        public Task ProcessExpiredCheckoutsAsync(CancellationToken cancellationToken);
        public Task CreatePaymentAsync(Guid checkoutId, CancellationToken cancellationToken);
        public Task AuthorizePaymentAsync(Guid checkoutId, CancellationToken cancellationToken);
        public Task CompletePaymentAsync(Guid checkoutId, CancellationToken cancellationToken);
        public Task FailPaymentAsync(Guid checkoutId, CancellationToken cancellationToken);
        public Task CancelPaymentAsync(Guid checkoutId, CancellationToken cancellationToken);
        public Task AbandonPaymentAsync(Guid checkoutId, CancellationToken cancellationToken);
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
