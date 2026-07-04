using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.Authentication
{
    public class LoginRequestDTO
    {
        [MaxLength(255)]
        [Required]
        public string Email { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
    }
}
