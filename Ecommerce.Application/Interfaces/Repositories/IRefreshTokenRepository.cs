using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        public Task<IEnumerable<RefreshToken>> GetAllAsync();
        public Task<IEnumerable<RefreshToken>> GetAllByUserIdAsync(Guid userId);
        public Task<RefreshToken?> GetActiveByUserIdAsync(Guid userId);
        public Task<RefreshToken?> GetByIdAsync(Guid id);
        public Task<RefreshToken?> GetByTokenAsync(string token);
        public void Add(RefreshToken refreshToken);
        public void Remove(RefreshToken refreshToken);
    }
}
