using Ecommerce.Application.DTOs.Authentication;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO request, CancellationToken cancellationToken);
        public Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request, CancellationToken cancellationToken);
        public Task<AuthResponseDTO> RefreshTokenAsync(string token, CancellationToken cancellationToken);
        public Task LogoutAsync(string token, CancellationToken cancellationToken);
    }
}
