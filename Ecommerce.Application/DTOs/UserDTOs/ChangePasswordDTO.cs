using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.UserDTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public string CurrentPassword { get; set; } = "";
        [Required]
        public string NewPassword { get; set; } = "";
    }
}
