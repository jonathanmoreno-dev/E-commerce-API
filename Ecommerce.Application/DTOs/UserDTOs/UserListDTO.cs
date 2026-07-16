using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.DTOs.UserDTOs
{
    public class UserListDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = "";
        public UserRole Role { get; set; }
    }
}
