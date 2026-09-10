using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.ValueObjects;
using Ecommerce.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Tests.Unit.Infrastructure.Authentication
{
    public class TokenServiceTests
    {
        private const string Key = "super-secret-key-for-tests-12345678901234567890";
        private const string Issuer = "EcommerceAPI";
        private const string Audience = "EcommerceClient";

        [Fact]
        public void ShouldGenerateAccessTokenWithExpectedClaims()
        {
            var user = CreateUser();
            user.ChangeRole(UserRole.Admin);
            var service = CreateService();

            var accessToken = service.GenerateAccessToken(user);

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

            Assert.Contains(jwtToken.Claims, x => x.Value == user.Id.ToString());
            Assert.Contains(jwtToken.Claims, x => x.Value == user.FullName.Value);
            Assert.Contains(jwtToken.Claims, x => x.Value == user.Email.Value);
            Assert.Contains(jwtToken.Claims, x => x.Value == user.Role.ToString());
        }

        [Fact]
        public void ShouldGenerateAccessTokenWithExpectedIssuerAudienceAndExpiration()
        {
            var user = CreateUser();
            var service = CreateService();
            var beforeGeneration = DateTime.UtcNow.AddMinutes(15);

            var accessToken = service.GenerateAccessToken(user);

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

            Assert.Equal(Issuer, jwtToken.Issuer);
            Assert.Contains(Audience, jwtToken.Audiences);
            Assert.InRange(jwtToken.ValidTo, beforeGeneration.AddSeconds(-5),beforeGeneration.AddSeconds(5));
        }

        [Fact]
        public void ShouldGenerateAccessTokenWithHmacSha256Signature()
        {
            var service = CreateService();

            var accessToken = service.GenerateAccessToken(CreateUser());

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

            Assert.Equal(SecurityAlgorithms.HmacSha256, jwtToken.Header.Alg);
        }

        [Fact]
        public void ShouldGenerateAccessTokenWithValidSignature()
        {
            var service = CreateService();
            var accessToken = service.GenerateAccessToken(CreateUser());
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)),
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(5)
            };

            tokenHandler.ValidateToken(accessToken, validationParameters, out _);
        }

        [Fact]
        public void ShouldGenerateRefreshTokenWith64RandomBytes()
        {
            var service = CreateService();

            var refreshToken = service.GenerateRefreshToken();

            var bytes = Convert.FromBase64String(refreshToken);

            Assert.Equal(64, bytes.Length);
        }

        [Fact]
        public void ShouldGenerateDifferentRefreshTokens()
        {
            var service = CreateService();

            var firstRefreshToken = service.GenerateRefreshToken();
            var secondRefreshToken = service.GenerateRefreshToken();

            Assert.NotEqual(firstRefreshToken, secondRefreshToken);
        }

        [Fact]
        public void ShouldReturnRefreshTokenExpirationAccordingToConfiguration()
        {
            var service = CreateService();
            var beforeGeneration = DateTime.UtcNow.AddDays(7);

            var expiration = service.GetRefreshTokenExpiration();

            Assert.InRange(expiration,beforeGeneration.AddSeconds(-5), beforeGeneration.AddSeconds(5));
        }

        private static TokenService CreateService()
        {
            var jwtOptions = Options.Create(new JwtOptions
            {
                Key = Key,
                Issuer = Issuer,
                Audience = Audience,
                AccessTokenExpirationMinutes = 15,
                RefreshTokenExpirationDays = 7
            });

            return new TokenService(jwtOptions);
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