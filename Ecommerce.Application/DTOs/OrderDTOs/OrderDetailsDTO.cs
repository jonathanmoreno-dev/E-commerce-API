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
        public ShippingDetailsDTO ShippingDetailsDTO { get; set; } = null!;
        public PaymentSummaryDTO PaymentSummaryDTO { get; set; } = null!;
        public List<OrderItemDetailsDTO> OrderItemDetailsDTOs { get; set; } = new();
    }
}
