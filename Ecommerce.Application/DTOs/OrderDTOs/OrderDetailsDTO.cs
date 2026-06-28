using Ecommerce.Application.DTOs.OrderItemDTOs;
using Ecommerce.Application.DTOs.PaymentDTOs;
using Ecommerce.Application.DTOs.ShippingDTOs;

namespace Ecommerce.Application.DTOs.OrderDTOs
{
    public class OrderDetailsDTO
    {
        public int Id { get; set; }
        public decimal Total { get; set; } 
        public string Status { get; set; } = "";
        public decimal ShippingCost { get; set; }
        public DateTime CreatedAt { get; set; }
        public ShippingDetailsDTO Shipping { get; set; } = null!;
        public PaymentSummaryDTO Payment { get; set; } = null!;
        public List<OrderItemDetailsDTO> OrderItems { get; set; } = new();
    }
}
