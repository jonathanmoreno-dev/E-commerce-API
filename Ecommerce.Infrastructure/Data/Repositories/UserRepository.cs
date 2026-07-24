using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<PagedList<User>> GetAllAsync(PaginationParams paginationParams)
        {
            var query = _appDbContext.Users.AsNoTracking();
            var totalItems = await query.CountAsync();
            var users = await query.OrderBy(x => x.FullName).Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync();

            return new PagedList<User>(users, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<PagedList<User>> GetAllByRoleAsync(UserRole role, PaginationParams paginationParams)
        {
            var query = _appDbContext.Users.Where(x => x.Role == role).AsNoTracking();
            var totalItems = await query.CountAsync();
            var users = await query.OrderBy(x => x.FullName).Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync();

            return new PagedList<User>(users, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(x => x.Email == new Email(email));
        }
        public async Task<bool> ExistsAdminAsync()
        {
            return await _appDbContext.Users.AnyAsync(x => x.Role == UserRole.Admin);
        }
        public void Add(User user)
        {
            _appDbContext.Users.Add(user);
        }
        public void Remove(User user)
        {
            _appDbContext.Users.Remove(user);
        }
    }
}
