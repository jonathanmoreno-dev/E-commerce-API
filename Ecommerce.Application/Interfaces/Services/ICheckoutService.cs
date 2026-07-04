using Ecommerce.Application.DTOs.CheckoutDTOs;
using Ecommerce.Application.DTOs.CheckoutItemDTOs;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICheckoutService
    {
        public Task<IEnumerable<CheckoutSummaryDTO>> GetAllActiveAsync();
        public Task<IEnumerable<CheckoutSummaryDTO>> GetAllActiveByUserIdAsync(Guid userId);
        public Task<IEnumerable<CheckoutSummaryDTO>> GetAllCurrentUserCheckoutsActiveAsync();
        public Task<CheckoutDetailsDTO> GetByIdAsync(Guid id);
        public Task<CheckoutDetailsDTO> CreateAsync(CheckoutCreateDTO checkoutCreate);
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
