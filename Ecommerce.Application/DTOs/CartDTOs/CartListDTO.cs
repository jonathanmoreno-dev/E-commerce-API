using Ecommerce.Application.DTOs.UserDTOs;

namespace Ecommerce.Application.DTOs.CartDTOs
{
    public class CartListDTO
    {
        public Guid Id { get; set; }
        public UserListDTO User { get; set; } = null!;
    }
}
