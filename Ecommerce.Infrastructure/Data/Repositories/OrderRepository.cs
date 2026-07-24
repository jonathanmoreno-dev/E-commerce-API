using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ecommerce.Infrastructure.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appDbContext;
        public OrderRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<PagedList<Order>> GetAllAsync(PaginationParams paginationParams)
        {
            var query = _appDbContext.Orders.AsNoTracking();
            var totalItems = await query.CountAsync();
            var orders = await query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync();

            return new PagedList<Order>(orders, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<PagedList<Order>> GetAllByUserIdAsync(Guid userId, PaginationParams paginationParams)
        {
            var query = _appDbContext.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Refunds).Include(x => x.OrderItems).ThenInclude(y => y.Product)
                .Where(x => x.UserId == userId).AsNoTracking();
            var totalItems = await query.CountAsync();
            var orders = await query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync();

            return new PagedList<Order>(orders, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<PagedList<Order>> GetAllByUserIdAndStatusAsync(Guid userId, OrderStatus status, PaginationParams paginationParams)
        {
            var query = _appDbContext.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Refunds).Include(x => x.OrderItems).ThenInclude(y => y.Product)
                .Where(x => x.Status == status && x.UserId == userId).AsNoTracking();
            var totalItems = await query.CountAsync();
            var orders = await query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync();

            return new PagedList<Order>(orders, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Order?> GetByIdForDetailsAsync(Guid id)
        {
            return await _appDbContext.Orders.Include(x => x.Shipping).Include(x => x.OrderItems).ThenInclude(x => x.Refunds).Include(x => x.OrderItems).ThenInclude(y => y.Product).FirstOrDefaultAsync(x => x.Id == id);
        }
        public void Add(Order order)
        {
            _appDbContext.Orders.Add(order);
        }
    }
}
