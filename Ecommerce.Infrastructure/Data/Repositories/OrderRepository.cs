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
        public async Task<PagedList<Order>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var query = _appDbContext.Orders.AsNoTracking();
            var totalItems = await query.CountAsync(cancellationToken);
            var orders = await query.OrderByDescending(x => x.CreatedAt).Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync(cancellationToken);

            return new PagedList<Order>(orders, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<PagedList<Order>> GetAllByUserIdAsync(Guid userId, PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var query = _appDbContext.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Refunds).Include(x => x.OrderItems).ThenInclude(y => y.Product)
                .Where(x => x.UserId == userId).AsNoTracking();
            var totalItems = await query.CountAsync(cancellationToken);
            var orders = await query.OrderByDescending(x => x.CreatedAt).Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync(cancellationToken);

            return new PagedList<Order>(orders, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<PagedList<Order>> GetAllByUserIdAndStatusAsync(Guid userId, OrderStatus status, PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var query = _appDbContext.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Refunds).Include(x => x.OrderItems).ThenInclude(y => y.Product)
                .Where(x => x.Status == status && x.UserId == userId).AsNoTracking();
            var totalItems = await query.CountAsync(cancellationToken);
            var orders = await query.OrderByDescending(x => x.CreatedAt).Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync(cancellationToken);

            return new PagedList<Order>(orders, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _appDbContext.Orders.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<Order?> GetByIdForDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _appDbContext.Orders.Include(x => x.Shipping).Include(x => x.OrderItems).ThenInclude(x => x.Refunds).Include(x => x.OrderItems).ThenInclude(y => y.Product).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public void Add(Order order)
        {
            _appDbContext.Orders.Add(order);
        }
    }
}
