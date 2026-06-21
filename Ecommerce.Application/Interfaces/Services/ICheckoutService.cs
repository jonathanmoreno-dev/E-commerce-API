using Ecommerce.Application.DTOs.CheckoutDTOs;
using Ecommerce.Application.DTOs.CheckoutItemDTOs;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICheckoutService
    {
        public Task<IEnumerable<CheckoutSummaryDTO>> GetAllCheckoutActivesAsync();
        public Task<CheckoutDetailsDTO> GetCheckoutActiveByUserIdAsync(int userId);
        public Task<CheckoutDetailsDTO> GetCheckoutByIdAsync(int id);
        public Task<CheckoutDetailsDTO> CreateCheckoutAsync(CheckoutCreateDTO checkoutCreate);
        public Task<CheckoutDetailsDTO> UpdateCheckoutAsync(int checkoutId, CheckoutUpdateDTO checkoutUpdate);
        public Task CreatePayment(int checkoutId);
        public Task AuthorizePaymentAsync(int checkoutId);
        public Task CompletePaymentAsync(int checkoutId);
        public Task FailPaymentAsync(int checkoutId);
        public Task CancelPaymentAsync(int checkoutId);
        public Task AbandonPaymentAsync(int checkoutId);
        public Task DeleteCheckoutAsync(int id);
    }
}
