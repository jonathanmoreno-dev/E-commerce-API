using System.ComponentModel.DataAnnotations;

namespace E_commerce_API.src.Application.DTOs.CheckoutItemDTOs
{
    public class CheckoutItemCreateDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
