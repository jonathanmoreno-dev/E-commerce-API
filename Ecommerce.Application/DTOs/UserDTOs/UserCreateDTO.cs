using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.UserDTOs
{
    public class UserCreateDTO
    {
        [Required]
        [MaxLength(150)]
        public string FullName { get; set; } = "";
        [MaxLength(255)]
        [Required]
        public string Email { get; set; } = "";
        [Required]
        [MaxLength(50)]
        public string PhoneNumber { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
    }
}
