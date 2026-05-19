using E_commerce_API.src.Application.DTOs.UserDTOs;

namespace E_commerce_API.src.Application.DTOs.CartDTOs
{
    public class CartListDTO
    {
        public int CartId { get; set; }
        public UserListDTO UserListDTO { get; set; } = null!;
    }
}
