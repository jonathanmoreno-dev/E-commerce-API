using E_commerce_API.src.Domain.Entities;

namespace E_commerce_API.src.Application.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        public Task<IEnumerable<Payment>> GetAllAsync();
        public Task<Payment?> GetByIdAsync(int id);
        public void Add(Payment payment);
        public void Update(Payment payment);
        public void Remove(Payment payment);
    }
}
