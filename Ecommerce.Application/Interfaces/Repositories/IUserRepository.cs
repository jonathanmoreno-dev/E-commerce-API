using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<PagedList<User>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<PagedList<User>> GetAllByRoleAsync(UserRole role, PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        public Task<bool> ExistsAdminAsync(CancellationToken cancellationToken);
        public void Add(User user);
        public void Remove(User user);
    }
}
