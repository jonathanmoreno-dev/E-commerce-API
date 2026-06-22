using Ecommerce.Application.DTOs.CheckoutDTOs;
using Ecommerce.Application.DTOs.CheckoutItemDTOs;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICheckoutService
    {
        public Task<IEnumerable<CheckoutSummaryDTO>> GetAllActiveAsync();
        public Task<IEnumerable<CheckoutSummaryDTO>> GetCurrentUserActiveAsync();
        public Task<IEnumerable<CheckoutSummaryDTO>> GetActiveByUserIdAsync(int userId);
        public Task<CheckoutDetailsDTO> GetByIdAsync(int id);
        public Task<CheckoutDetailsDTO> CreateAsync(CheckoutCreateDTO checkoutCreate);
        public Task<CheckoutDetailsDTO> UpdateAsync(int checkoutId, CheckoutUpdateDTO checkoutUpdate);
        public Task CreatePaymentAsync(int checkoutId);
        public Task AuthorizePaymentAsync(int checkoutId);
        public Task CompletePaymentAsync(int checkoutId);
        public Task FailPaymentAsync(int checkoutId);
        public Task CancelPaymentAsync(int checkoutId);
        public Task AbandonPaymentAsync(int checkoutId);
        public Task DeleteAsync(int id);
    }
}
