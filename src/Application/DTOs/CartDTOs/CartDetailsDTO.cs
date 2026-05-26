using E_commerce_API.src.Application.DTOs.CartItemDTOs;
using E_commerce_API.src.Application.DTOs.UserDTOs;

namespace E_commerce_API.src.Application.DTOs.CartDTOs
{
    public class CartDetailsDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CartItemListDTO> CartItemListDTOs { get; set; } = new();
    }
}
