using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface ICheckoutRepository
    {
        public Task<IEnumerable<Checkout>> GetAllActiveAsync();
        public Task<IEnumerable<Checkout>> GetAllActiveByUserIdAsync(int userId);
        public Task<Checkout?> GetByIdAsync(int id);
        public Task<Checkout?> GetByIdWithPaymentAttemptsAsync(int id);
        public void Add(Checkout checkout);
        public void Remove(Checkout checkout);
    }
}
