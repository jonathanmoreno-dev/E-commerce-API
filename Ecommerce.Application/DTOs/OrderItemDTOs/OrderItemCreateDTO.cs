using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.OrderItemDTOs
{
    public class OrderItemCreateDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
