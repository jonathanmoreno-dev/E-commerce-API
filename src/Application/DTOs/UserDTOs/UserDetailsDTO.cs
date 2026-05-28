using E_commerce_API.src.Application.DTOs.CartDTOs;
using E_commerce_API.src.Application.DTOs.OrderDTOs;

namespace E_commerce_API.src.Application.DTOs.UserDTOs
{
    public class UserDetailsDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public bool IsAdmin { get; set; }
        public string AvatarImageUrl { get; set; } = "";
        public CartDetailsDTO CartDetailsDTO { get; set; } = null!;
        public List<OrderListDTO> OrderListDTOs { get; set; } = new();
    }
}
