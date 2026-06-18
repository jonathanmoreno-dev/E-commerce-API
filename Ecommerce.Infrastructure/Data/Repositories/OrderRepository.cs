using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appDbContext;
        public OrderRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _appDbContext.Orders.AsNoTracking().ToListAsync();
        }
        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _appDbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
        }
        public void Add(Order order)
        {
            _appDbContext.Orders.Add(order);
        }
        public void Update(Order order)
        {
            _appDbContext.Orders.Update(order);
        }
        public void Remove(Order order)
        {
            _appDbContext.Orders.Remove(order);
        }
    }
}
