using Ecommerce.Application.DTOs.CheckoutDTOs;
using Ecommerce.Application.Interfaces.Services;

namespace Ecommerce.Application.Services
{
    public class CheckoutService : ICheckoutService
    {
        public Task<IEnumerable<CheckoutSummaryDTO>> GetAllActiveAsync()
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<CheckoutSummaryDTO>> GetAllCurrentUserCheckoutsActiveAsync()
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<CheckoutSummaryDTO>> GetAllActiveByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }
        public Task<CheckoutDetailsDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<CheckoutDetailsDTO> CreateAsync(CheckoutCreateDTO checkoutCreate)
        {
            throw new NotImplementedException();
        }
        public Task<CheckoutDetailsDTO> UpdateAsync(int checkoutId, CheckoutUpdateDTO checkoutUpdate)
        {
            throw new NotImplementedException();
        }
        public Task CreatePaymentAsync(int checkoutId)
        {
            throw new NotImplementedException();
        }
        public Task AuthorizePaymentAsync(int checkoutId)
        {
            throw new NotImplementedException();
        }
        public Task CompletePaymentAsync(int checkoutId)
        {
            throw new NotImplementedException();
        }
        public Task FailPaymentAsync(int checkoutId)
        {
            throw new NotImplementedException();
        }
        public Task CancelPaymentAsync(int checkoutId)
        {
            throw new NotImplementedException();
        }
        public Task AbandonPaymentAsync(int checkoutId)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
