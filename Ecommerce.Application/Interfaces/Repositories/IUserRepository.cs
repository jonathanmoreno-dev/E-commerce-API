using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<PagedList<User>> GetAllAsync(PaginationParams paginationParams);
        public Task<PagedList<User>> GetAllByRoleAsync(UserRole role, PaginationParams paginationParams);
        public Task<User?> GetByIdAsync(Guid id);
        public Task<User?> GetByEmailAsync(string email);
        public Task<bool> ExistsAdminAsync();
        public void Add(User user);
        public void Remove(User user);
    }
}
