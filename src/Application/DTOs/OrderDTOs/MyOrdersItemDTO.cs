using E_commerce_API.src.Application.DTOs.ProductDTOs;

namespace E_commerce_API.src.Application.DTOs.OrderDTOs
{
    public class MyOrdersItemDTO
    {
        public int OrderId { get; set; }
        public int OrderItemId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = "";
        public ProductSummaryDTO ProductSummaryDTO { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
