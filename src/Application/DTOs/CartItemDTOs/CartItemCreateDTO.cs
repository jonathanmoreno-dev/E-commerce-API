using System.ComponentModel.DataAnnotations;

namespace E_commerce_API.src.Application.DTOs.CartItemDTOs
{
    public class CartItemCreateDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
