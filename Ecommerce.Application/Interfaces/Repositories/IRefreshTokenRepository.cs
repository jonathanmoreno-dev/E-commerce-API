using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        public Task<PagedList<RefreshToken>> GetAllAsync(PaginationParams paginationParams);
        public Task<PagedList<RefreshToken>> GetAllByUserIdAsync(Guid userId, PaginationParams paginationParams);
        public Task<RefreshToken?> GetActiveByUserIdAsync(Guid userId);
        public Task<RefreshToken?> GetByIdAsync(Guid id);
        public Task<RefreshToken?> GetByTokenAsync(string token);
        public void Add(RefreshToken refreshToken);
        public void Remove(RefreshToken refreshToken);
    }
}
