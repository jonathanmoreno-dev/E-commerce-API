using Ecommerce.Domain.Enums;
using Ecommerce.Application.DTOs.OrderItemDTOs;

namespace Ecommerce.Application.DTOs.OrderDTOs
{
    public class OrderListItemDTO
    {
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public OrderItemSummaryDTO OrderItems { get; set; } = null!;
    }
}
