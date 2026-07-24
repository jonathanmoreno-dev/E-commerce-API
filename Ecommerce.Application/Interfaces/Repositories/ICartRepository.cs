using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        public Task<PagedList<Cart>> GetAllAsync(PaginationParams paginationParams);
        public Task<Cart?> GetByIdAsync(Guid id);
        public Task<Cart?> GetByUserIdAsync(Guid userId);
        public void Add(Cart cart);
        public void Remove(Cart cart);
    }
}
