using System.ComponentModel.DataAnnotations;

namespace E_commerce_API.src.Application.DTOs.CartItemDTOs
{
    public class CartItemCreateDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
