using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
namespace Ecommerce.Infrastructure.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly SymmetricSecurityKey _key;
        private readonly SigningCredentials _credentials;
        public TokenService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            _tokenHandler = new();
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            _credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        }
        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.FullName.Value),
                new(ClaimTypes.Email, user.Email.Value)
            };
            if (user.IsAdmin)
                claims.Add(new(ClaimTypes.Role, "Admin"));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes),
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = _credentials
            };
            var acessToken = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(acessToken);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
        public DateTime GetRefreshTokenExpiration()
        {
            return DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);
        }
    }
}
