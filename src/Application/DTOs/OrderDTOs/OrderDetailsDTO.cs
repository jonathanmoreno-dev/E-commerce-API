using E_commerce_API.src.Application.DTOs.OrderItemDTOs;
using E_commerce_API.src.Application.DTOs.PaymentDTOs;
using E_commerce_API.src.Application.DTOs.ShippingDTOs;

namespace E_commerce_API.src.Application.DTOs.OrderDTOs
{
    public class OrderDetailsDTO
    {
        public int Id { get; set; }
        public decimal Total { get; set; } 
        public string Status { get; set; } = "";
        public decimal ShippingCost { get; set; }
        public DateTime CreatedAt { get; set; }
        public ShippingSummaryDTO? ShippingSummaryDTO { get; set; } = null!;
        public PaymentSummaryDTO? PaymentSummaryDTO { get; set; } = null!;
        public List<OrderItemDetailsDTO> OrderItemListDTOs { get; set; } = new();
    }
}
