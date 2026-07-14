using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.Authentication
{
    public class RefreshTokenRequestDTO
    {
        [Required]
        public string RefreshToken { get; set; } = "";
    }
}
