using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        public AuthService(ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, ICartRepository cartRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
        {
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }
        public async Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO request)
        {
            var userExists = await _userRepository.GetByEmailAsync(request.Email);
            if (userExists is not null)
                throw new ArgumentException("Email already registered");

            var user = new User(new PersonName(request.FullName), new Email(request.Email), new PhoneNumber(request.PhoneNumber), _passwordHasher.HashPassword(request.Password));
            var cart = new Cart(user.Id);

            _userRepository.Add(user);
            _cartRepository.Add(cart);

            var refreshToken = new RefreshToken(user.Id, _tokenService.GenerateRefreshToken(), _tokenService.GetRefreshTokenExpiration());

            _refreshTokenRepository.Add(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            var authResponseDTO = new AuthResponseDTO()
            {
                AccessToken = _tokenService.GenerateAccessToken(user),
                RefreshToken = refreshToken.Token
            };
            return authResponseDTO;
        }
        public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null)
                throw new UnauthorizedAccessException("Invalid credentials");

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            var refreshTokenExists = await _refreshTokenRepository.GetActiveByUserIdAsync(user.Id);
            if(refreshTokenExists is not null)
                refreshTokenExists.Revoke();

            var refreshToken = new RefreshToken(user.Id, _tokenService.GenerateRefreshToken(), _tokenService.GetRefreshTokenExpiration());

            _refreshTokenRepository.Add(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            var authResponseDTO = new AuthResponseDTO()
            {
                AccessToken = _tokenService.GenerateAccessToken(user),
                RefreshToken = refreshToken.Token
            };
            return authResponseDTO;
        }
        public async Task<AuthResponseDTO> RefreshTokenAsync(string token)
        {
            var refreshTokenExists = await _refreshTokenRepository.GetByTokenAsync(token);
            if(refreshTokenExists is null)
                throw new UnauthorizedAccessException("Invalid refresh token");
            if (refreshTokenExists.IsExpired)
                throw new UnauthorizedAccessException("Refresh token expired");
            if(refreshTokenExists.IsRevoked)
                throw new UnauthorizedAccessException("Refresh token revoked");

            refreshTokenExists.Revoke();

            var refreshToken = new RefreshToken(refreshTokenExists.UserId, _tokenService.GenerateRefreshToken(), _tokenService.GetRefreshTokenExpiration());

            _refreshTokenRepository.Add(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            var authResponseDTO = new AuthResponseDTO()
            {
                AccessToken = _tokenService.GenerateAccessToken(refreshToken.User),
                RefreshToken = refreshToken.Token
            };
            return authResponseDTO;
        }
        public async Task LogoutAsync(string token)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(token);
            if(refreshToken is null)
                return;

            refreshToken.Revoke();
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
