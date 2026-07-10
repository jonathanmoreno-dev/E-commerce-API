namespace Ecommerce.Application.DTOs.Authentication
{
    public class AuthResponseDTO
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
}
