using Ecommerce.Application.DTOs.UserDTOs;
using Ecommerce.Application.DTOs.CartItemDTOs;

namespace Ecommerce.Application.DTOs.CartDTOs
{
    public class CartDetailsDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CartItemListDTO> CartItems { get; set; } = new();
        public decimal SubTotal { get; set; }
    }
}
