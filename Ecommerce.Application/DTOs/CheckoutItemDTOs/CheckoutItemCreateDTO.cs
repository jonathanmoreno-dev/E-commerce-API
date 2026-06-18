using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.CheckoutItemDTOs
{
    public class CheckoutItemCreateDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
