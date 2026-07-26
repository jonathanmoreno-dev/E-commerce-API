using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _appDbContext;
        public RefreshTokenRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<PagedList<RefreshToken>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var query = _appDbContext.RefreshTokens.AsNoTracking();
            var totalItems = await query.CountAsync(cancellationToken);
            var refreshTokens = await query.OrderByDescending(x => x.CreatedAt).Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync(cancellationToken);

            return new PagedList<RefreshToken>(refreshTokens, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<PagedList<RefreshToken>> GetAllByUserIdAsync(Guid userId, PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var query = _appDbContext.RefreshTokens.Where(x => x.UserId == userId).AsNoTracking();
            var totalItems = await query.CountAsync(cancellationToken);
            var refreshTokens = await query.OrderByDescending(x => x.CreatedAt).Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync(cancellationToken);

            return new PagedList<RefreshToken>(refreshTokens, paginationParams.PageNumber, paginationParams.PageSize, totalItems);
        }
        public async Task<RefreshToken?> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _appDbContext.RefreshTokens.Where(x => x.UserId == userId).FirstOrDefaultAsync(x => !(DateTime.UtcNow < x.ExpiresAt) && (x.RevokedAt == null), cancellationToken);
        }
        public async Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _appDbContext.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
        {
            return await _appDbContext.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
        }
        public void Add(RefreshToken refreshToken)
        {
            _appDbContext.RefreshTokens.Add(refreshToken);
        }
        public void Remove(RefreshToken refreshToken)
        {
            _appDbContext.RefreshTokens.Remove(refreshToken);
        }
    }
}
