using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.UserDTOs
{
    public class UserUpdateDTO
    {
        [MaxLength(150)]
        public string? FullName { get; set; }
        [MaxLength(255)]
        public string? Email { get; set; } 
        [MaxLength(50)]
        public string? PhoneNumber { get; set; } 
        public string? Password { get; set; }
        public string? AvatarImageUrl { get; set; }
    }
}
