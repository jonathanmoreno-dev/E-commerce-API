using Ecommerce.Application.DTOs.UserDTOs;

namespace Ecommerce.Application.DTOs.CartDTOs
{
    public class CartListDTO
    {
        public int Id { get; set; }
        public UserListDTO UserListDTO { get; set; } = null!;
    }
}
