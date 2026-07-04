using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface ICheckoutRepository
    {
        public Task<IEnumerable<Checkout>> GetAllActiveAsync();
        public Task<IEnumerable<Checkout>> GetAllActiveByUserIdAsync(Guid userId);
        public Task<Checkout?> GetByIdAsync(Guid id);
        public Task<Checkout?> GetByIdWithPaymentAttemptsAsync(Guid id);
        public void Add(Checkout checkout);
        public void Remove(Checkout checkout);
    }
}
