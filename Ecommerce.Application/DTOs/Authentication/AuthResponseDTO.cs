namespace Ecommerce.Application.DTOs.Authentication
{
    public class AuthResponseDTO
    {
        public string AcessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
}
