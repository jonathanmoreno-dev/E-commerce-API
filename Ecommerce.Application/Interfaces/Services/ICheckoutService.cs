using Ecommerce.Application.DTOs.CheckoutDTOs;
using Ecommerce.Application.DTOs.CheckoutItemDTOs;
using Ecommerce.Application.Pagination;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICheckoutService
    {
        public Task<PagedList<CheckoutSummaryDTO>> GetAllActiveAsync(PaginationParams paginationParams);
        public Task<PagedList<CheckoutSummaryDTO>> GetAllActiveByUserIdAsync(Guid userId, PaginationParams paginationParams);
        public Task<PagedList<CheckoutSummaryDTO>> GetAllCurrentUserCheckoutsActiveAsync(PaginationParams paginationParams);
        public Task<CheckoutDetailsDTO> GetByIdAsync(Guid id);
        public Task<CheckoutDetailsDTO> CreateAsync();
        public Task<CheckoutDetailsDTO> UpdateAsync(Guid checkoutId, CheckoutUpdateDTO checkoutUpdate);
        public Task CreatePaymentAsync(Guid checkoutId);
        public Task AuthorizePaymentAsync(Guid checkoutId);
        public Task CompletePaymentAsync(Guid checkoutId);
        public Task FailPaymentAsync(Guid checkoutId);
        public Task CancelPaymentAsync(Guid checkoutId);
        public Task AbandonPaymentAsync(Guid checkoutId);
        public Task DeleteAsync(Guid id);
    }
}
