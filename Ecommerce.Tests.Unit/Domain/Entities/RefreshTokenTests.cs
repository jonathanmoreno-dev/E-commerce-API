using Ecommerce.Domain.Entities;

namespace Ecommerce.Tests.Unit.Domain.Entities
{
    public class RefreshTokenTests
    {
        [Fact]
        public void ShouldCreateRefreshTokenWithValidValues()
        {
            var userId = Guid.NewGuid();
            var token = "refresh-token";
            var expiresAt = DateTime.UtcNow.AddDays(7);

            var refreshToken = new RefreshToken(userId, token, expiresAt);

            Assert.NotEqual(Guid.Empty, refreshToken.Id);
            Assert.Equal(userId, refreshToken.UserId);
            Assert.Equal(token, refreshToken.Token);
            Assert.Equal(expiresAt, refreshToken.ExpiresAt);
            Assert.NotEqual(DateTime.MinValue, refreshToken.CreatedAt);
            Assert.Null(refreshToken.RevokedAt);
            Assert.False(refreshToken.IsExpired);
            Assert.False(refreshToken.IsRevoked);
            Assert.True(refreshToken.IsActive);
        }

        [Fact]
        public void ShouldBeExpiredWhenExpirationDateIsInThePast()
        {
            var refreshToken = CreateRefreshToken(DateTime.UtcNow.AddSeconds(-1));

            Assert.True(refreshToken.IsExpired);
            Assert.False(refreshToken.IsActive);
        }

        [Fact]
        public void ShouldRevokeRefreshToken()
        {
            var refreshToken = CreateRefreshToken(DateTime.UtcNow.AddDays(7));

            refreshToken.Revoke();

            Assert.NotNull(refreshToken.RevokedAt);
            Assert.True(refreshToken.IsRevoked);
            Assert.False(refreshToken.IsActive);
        }

        [Fact]
        public void ShouldRemainInactiveWhenExpiredAndRevoked()
        {
            var refreshToken = CreateRefreshToken(DateTime.UtcNow.AddSeconds(-1));

            refreshToken.Revoke();

            Assert.True(refreshToken.IsExpired);
            Assert.True(refreshToken.IsRevoked);
            Assert.False(refreshToken.IsActive);
        }

        private static RefreshToken CreateRefreshToken(DateTime expiresAt)
        {
            return new RefreshToken(Guid.NewGuid(), "refresh-token", expiresAt);
        }
    }
}
