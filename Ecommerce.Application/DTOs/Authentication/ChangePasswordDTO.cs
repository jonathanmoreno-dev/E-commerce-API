using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.Authentication
{
    public class ChangePasswordDTO
    {
        [Required]
        public string CurrentPassword { get; set; } = "";
        [Required]
        public string NewPassword { get; set; } = "";
    }
}
