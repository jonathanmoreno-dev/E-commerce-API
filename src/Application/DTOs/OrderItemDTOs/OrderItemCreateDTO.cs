using System.ComponentModel.DataAnnotations;

namespace E_commerce_API.src.Application.DTOs.OrderItemDTOs
{
    public class OrderItemCreateDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
