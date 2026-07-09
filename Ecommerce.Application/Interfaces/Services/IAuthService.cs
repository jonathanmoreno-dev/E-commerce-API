using Ecommerce.Application.DTOs.Authentication;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO request);
        public Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request);
        public Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken);
        public Task LogoutAsync(string refreshToken);
    }
}
