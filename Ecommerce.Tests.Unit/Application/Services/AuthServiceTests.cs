using System.Threading;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;
using Moq;

namespace Ecommerce.Tests.Unit.Application.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<ICartRepository> _cartRepositoryMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

        [Fact]
        public async Task ShouldRegisterUserAndReturnAuthResponse()
        {
            var request = CreateRegisterRequest();
            var cancellationToken = new CancellationToken();
            User? user = null;
            Cart? cart = null;
            RefreshToken? refreshToken = null;
            var expiration = DateTime.UtcNow.AddDays(7);

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email, cancellationToken)).ReturnsAsync((User?)null);
            _passwordHasherMock.Setup(x => x.HashPassword(request.Password)).Returns("hashed-password");
            _tokenServiceMock.Setup(x => x.GenerateRefreshToken()).Returns("refresh-token");
            _tokenServiceMock.Setup(x => x.GetRefreshTokenExpiration()).Returns(expiration);
            _tokenServiceMock.Setup(x => x.GenerateAccessToken(It.IsAny<User>())).Returns("access-token");
            _userRepositoryMock
                .Setup(x => x.Add(It.IsAny<User>()))
                .Callback<User>(addedUser => user = addedUser);
            _cartRepositoryMock
                .Setup(x => x.Add(It.IsAny<Cart>()))
                .Callback<Cart>(addedCart => cart = addedCart);
            _refreshTokenRepositoryMock
                .Setup(x => x.Add(It.IsAny<RefreshToken>()))
                .Callback<RefreshToken>(addedRefreshToken => refreshToken = addedRefreshToken);

            var service = CreateService();

            var response = await service.RegisterAsync(request, cancellationToken);

            Assert.Equal("access-token", response.AccessToken);
            Assert.Equal("refresh-token", response.RefreshToken);
            Assert.NotNull(user);
            Assert.NotNull(cart);
            Assert.NotNull(refreshToken);
            Assert.Equal(user.Id, cart.UserId);
            Assert.Equal(user.Id, refreshToken.UserId);
            Assert.Equal(expiration, refreshToken.ExpiresAt);

            _userRepositoryMock.Verify(x => x.GetByEmailAsync(request.Email, cancellationToken), Times.Once);
            _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
            _cartRepositoryMock.Verify(x => x.Add(It.IsAny<Cart>()), Times.Once);
            _refreshTokenRepositoryMock.Verify(x => x.Add(It.IsAny<RefreshToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowConflictExceptionWhenRegisteringAnExistingEmail()
        {
            var request = CreateRegisterRequest();
            var existingUser = CreateUser();
            var cancellationToken = new CancellationToken();

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(request.Email, cancellationToken))
                .ReturnsAsync(existingUser);

            var service = CreateService();

            await Assert.ThrowsAsync<ConflictException>(() => service.RegisterAsync(request, cancellationToken));

            _passwordHasherMock.Verify(x => x.HashPassword(It.IsAny<string>()), Times.Never);
            _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
            _cartRepositoryMock.Verify(x => x.Add(It.IsAny<Cart>()), Times.Never);
            _refreshTokenRepositoryMock.Verify(x => x.Add(It.IsAny<RefreshToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldLoginUserAndReturnAuthResponse()
        {
            var request = CreateLoginRequest();
            var user = CreateUser();
            var cancellationToken = new CancellationToken();
            var expiration = DateTime.UtcNow.AddDays(7);

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(request.Email, cancellationToken))
                .ReturnsAsync(user);
            _passwordHasherMock
                .Setup(x => x.VerifyPassword(request.Password, user.PasswordHash))
                .Returns(true);
            _refreshTokenRepositoryMock
                .Setup(x => x.GetActiveByUserIdAsync(user.Id, cancellationToken))
                .ReturnsAsync((RefreshToken?)null);
            _tokenServiceMock
                .Setup(x => x.GenerateRefreshToken())
                .Returns("refresh-token");
            _tokenServiceMock
                .Setup(x => x.GetRefreshTokenExpiration())
                .Returns(expiration);
            _tokenServiceMock
                .Setup(x => x.GenerateAccessToken(user))
                .Returns("access-token");

            var service = CreateService();

            var response = await service.LoginAsync(request, cancellationToken);

            Assert.Equal("access-token", response.AccessToken);
            Assert.Equal("refresh-token", response.RefreshToken);

            _userRepositoryMock.Verify(x => x.GetByEmailAsync(request.Email, cancellationToken), Times.Once);
            _passwordHasherMock.Verify(x => x.VerifyPassword(request.Password, user.PasswordHash), Times.Once);
            _refreshTokenRepositoryMock.Verify(x => x.GetActiveByUserIdAsync(user.Id, cancellationToken), Times.Once);
            _refreshTokenRepositoryMock.Verify(x => x.Add(It.Is<RefreshToken>(token => token.UserId == user.Id)), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowUnauthorizedExceptionWhenLoginUserDoesNotExist()
        {
            var request = CreateLoginRequest();
            var cancellationToken = new CancellationToken();

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email, cancellationToken)).ReturnsAsync((User?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<UnauthorizedException>(() => service.LoginAsync(request, cancellationToken));

            _passwordHasherMock.Verify(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _refreshTokenRepositoryMock.Verify(x => x.Add(It.IsAny<RefreshToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowUnauthorizedExceptionWhenLoginPasswordIsInvalid()
        {
            var request = CreateLoginRequest();
            var user = CreateUser();
            var cancellationToken = new CancellationToken();

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email, cancellationToken)).ReturnsAsync(user);
            _passwordHasherMock.Setup(x => x.VerifyPassword(request.Password, user.PasswordHash)).Returns(false);

            var service = CreateService();

            await Assert.ThrowsAsync<UnauthorizedException>(() => service.LoginAsync(request, cancellationToken));

            _refreshTokenRepositoryMock.Verify(x => x.GetActiveByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _refreshTokenRepositoryMock.Verify(x => x.Add(It.IsAny<RefreshToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldRevokeExistingActiveRefreshTokenWhenLoggingIn()
        {
            var request = CreateLoginRequest();
            var user = CreateUser();
            var activeRefreshToken = new RefreshToken(user.Id, "old-refresh-token", DateTime.UtcNow.AddDays(7));
            var cancellationToken = new CancellationToken();

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email, cancellationToken)).ReturnsAsync(user);
            _passwordHasherMock.Setup(x => x.VerifyPassword(request.Password, user.PasswordHash)).Returns(true);
            _refreshTokenRepositoryMock.Setup(x => x.GetActiveByUserIdAsync(user.Id, cancellationToken))
                .ReturnsAsync(activeRefreshToken);
            _tokenServiceMock.Setup(x => x.GenerateRefreshToken()).Returns("new-refresh-token");
            _tokenServiceMock.Setup(x => x.GetRefreshTokenExpiration()).Returns(DateTime.UtcNow.AddDays(7));
            _tokenServiceMock.Setup(x => x.GenerateAccessToken(user)).Returns("access-token");

            var service = CreateService();

            await service.LoginAsync(request, cancellationToken);

            Assert.True(activeRefreshToken.IsRevoked);

            _refreshTokenRepositoryMock.Verify(x => x.Add(It.IsAny<RefreshToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowUnauthorizedExceptionWhenRefreshTokenDoesNotExist()
        {
            var cancellationToken = new CancellationToken();

            _refreshTokenRepositoryMock
                .Setup(x => x.GetByTokenAsync("invalid-token", cancellationToken))
                .ReturnsAsync((RefreshToken?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<UnauthorizedException>(() => service.RefreshTokenAsync("invalid-token", cancellationToken));

            _refreshTokenRepositoryMock.Verify(x => x.Add(It.IsAny<RefreshToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowUnauthorizedExceptionWhenRefreshTokenIsExpired()
        {
            var cancellationToken = new CancellationToken();
            var expiredRefreshToken = new RefreshToken(Guid.NewGuid(), "expired-token", DateTime.UtcNow.AddSeconds(-1));

            _refreshTokenRepositoryMock
                .Setup(x => x.GetByTokenAsync(expiredRefreshToken.Token, cancellationToken))
                .ReturnsAsync(expiredRefreshToken);

            var service = CreateService();

            await Assert.ThrowsAsync<UnauthorizedException>(() => service.RefreshTokenAsync(
                expiredRefreshToken.Token,
                cancellationToken));
        }

        [Fact]
        public async Task ShouldThrowUnauthorizedExceptionWhenRefreshTokenIsRevoked()
        {
            var cancellationToken = new CancellationToken();
            var revokedRefreshToken = new RefreshToken(Guid.NewGuid(), "revoked-token", DateTime.UtcNow.AddDays(7));
            revokedRefreshToken.Revoke();

            _refreshTokenRepositoryMock
                .Setup(x => x.GetByTokenAsync(revokedRefreshToken.Token, cancellationToken))
                .ReturnsAsync(revokedRefreshToken);

            var service = CreateService();

            await Assert.ThrowsAsync<UnauthorizedException>(() => service.RefreshTokenAsync(
                revokedRefreshToken.Token,
                cancellationToken));
        }

        [Fact]
        public async Task ShouldRefreshTokenAndReturnAuthResponse()
        {
            var user = CreateUser();
            var cancellationToken = new CancellationToken();
            var currentRefreshToken = new RefreshToken(user.Id, "current-token", DateTime.UtcNow.AddDays(7));
            var expiration = DateTime.UtcNow.AddDays(7);

            _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync(currentRefreshToken.Token, cancellationToken))
                .ReturnsAsync(currentRefreshToken);
            _tokenServiceMock.Setup(x => x.GenerateRefreshToken()).Returns("new-refresh-token");
            _tokenServiceMock.Setup(x => x.GetRefreshTokenExpiration()).Returns(expiration);
            _tokenServiceMock.Setup(x => x.GenerateAccessToken(currentRefreshToken.User)).Returns("access-token");

            var service = CreateService();

            var response = await service.RefreshTokenAsync(currentRefreshToken.Token, cancellationToken);

            Assert.Equal("access-token", response.AccessToken);
            Assert.Equal("new-refresh-token", response.RefreshToken);
            Assert.True(currentRefreshToken.IsRevoked);

            _refreshTokenRepositoryMock.Verify(x => x.Add(It.Is<RefreshToken>(token => token.UserId == currentRefreshToken.UserId)), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldRevokeRefreshTokenWhenLoggingOut()
        {
            var cancellationToken = new CancellationToken();
            var refreshToken = new RefreshToken(
                Guid.NewGuid(),
                "refresh-token",
                DateTime.UtcNow.AddDays(7));

            _refreshTokenRepositoryMock
                .Setup(x => x.GetByTokenAsync(refreshToken.Token, cancellationToken))
                .ReturnsAsync(refreshToken);

            var service = CreateService();

            await service.LogoutAsync(refreshToken.Token, cancellationToken);

            Assert.True(refreshToken.IsRevoked);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldDoNothingWhenLoggingOutWithAnUnknownToken()
        {
            var cancellationToken = new CancellationToken();

            _refreshTokenRepositoryMock
                .Setup(x => x.GetByTokenAsync("unknown-token", cancellationToken))
                .ReturnsAsync((RefreshToken?)null);

            var service = CreateService();

            await service.LogoutAsync("unknown-token", cancellationToken);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        private AuthService CreateService()
        {
            return new AuthService(
                _tokenServiceMock.Object,
                _refreshTokenRepositoryMock.Object,
                _userRepositoryMock.Object,
                _cartRepositoryMock.Object,
                _passwordHasherMock.Object,
                _unitOfWorkMock.Object);
        }

        private static RegisterRequestDTO CreateRegisterRequest()
        {
            return new RegisterRequestDTO
            {
                FullName = "Maria da Silva",
                Email = "user@example.com",
                PhoneNumber = "+5538992157062",
                Password = "Password@123"
            };
        }

        private static LoginRequestDTO CreateLoginRequest()
        {
            return new LoginRequestDTO
            {
                Email = "user@example.com",
                Password = "Password@123"
            };
        }

        private static User CreateUser()
        {
            return new User(
                new PersonName("Maria da Silva"),
                new Email("user@example.com"),
                new PhoneNumber("+5538992157062"),
                "hashed-password");
        }
    }
}
