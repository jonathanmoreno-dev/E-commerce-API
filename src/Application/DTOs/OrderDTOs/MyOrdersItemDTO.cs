using E_commerce_API.src.Application.DTOs.OrderItemDTOs;
using E_commerce_API.src.Application.DTOs.ProductDTOs;
using E_commerce_API.src.Domain.Enums;

namespace E_commerce_API.src.Application.DTOs.OrderDTOs
{
    public class MyOrdersItemDTO
    {
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public OrderItemSummaryDTO OrderItemSummaryDTO { get; set; } = null!;
    }
}
