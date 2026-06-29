using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
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
        public async Task<IEnumerable<Order>> GetAllByUserIdAsync(int userId)
        {
            return await _appDbContext.Orders.Include(x => x.OrderItems).ThenInclude(y => y.Product).Where(x => x.UserId == userId).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<Order>> GetAllByStatusAsync(OrderStatus status)
        {
            return await _appDbContext.Orders.Include(x => x.OrderItems).ThenInclude(y => y.Product).Where(x => x.Status == status).AsNoTracking().ToListAsync();
        }
        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _appDbContext.Orders.Include(x => x.Shipping).Include(x => x.OrderItems).ThenInclude(y => y.Product).FirstOrDefaultAsync(x => x.Id == id);
        }
        public void Add(Order order)
        {
            _appDbContext.Orders.Add(order);
        }
    }
}
