using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.CheckoutItemDTOs
{
    public class CheckoutItemCreateDTO
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
