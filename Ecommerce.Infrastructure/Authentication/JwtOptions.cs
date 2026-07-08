namespace Ecommerce.Infrastructure.Authentication
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";
        public string Key { get; set; } = "";
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }

    }
}
