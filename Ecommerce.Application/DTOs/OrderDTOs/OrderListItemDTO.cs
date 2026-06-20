using Ecommerce.Domain.Enums;
using Ecommerce.Application.DTOs.OrderItemDTOs;

namespace Ecommerce.Application.DTOs.OrderDTOs
{
    public class OrderSummaryDTO
    {
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public OrderItemSummaryDTO OrderItemSummaryDTO { get; set; } = null!;
    }
}
