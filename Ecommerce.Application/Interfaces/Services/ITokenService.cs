using System.Security.Claims;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ITokenService
    {
        public string GenerateAccessToken(User user);
        public string GenerateRefreshToken();
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string tokenExpired);
    }
}
