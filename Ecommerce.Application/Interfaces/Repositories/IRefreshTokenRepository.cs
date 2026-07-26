using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        public Task<PagedList<RefreshToken>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<PagedList<RefreshToken>> GetAllByUserIdAsync(Guid userId, PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<RefreshToken?> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        public Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);
        public void Add(RefreshToken refreshToken);
        public void Remove(RefreshToken refreshToken);
    }
}
