using Ecommerce.Application.Interfaces.Repositories;
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
        public async Task<IEnumerable<RefreshToken>> GetAllAsync()
        {
            return await _appDbContext.RefreshTokens.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<RefreshToken>> GetAllByUserIdAsync(Guid userId)
        {
            return await _appDbContext.RefreshTokens.Where(x => x.UserId == userId).AsNoTracking().ToListAsync();
        }
        public async Task<RefreshToken?> GetActiveByUserIdAsync(Guid userId)
        {
            return await _appDbContext.RefreshTokens.Where(x => x.UserId == userId).FirstOrDefaultAsync(x => !(DateTime.UtcNow < x.ExpiresAt) && (x.RevokedAt == null));
        }
        public async Task<RefreshToken?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _appDbContext.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == token);
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
