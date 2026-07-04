using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.CartItemDTOs
{
    public class CartItemCreateDTO
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
