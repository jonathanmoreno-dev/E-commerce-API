using E_commerce_API.src.Application.DTOs.ProductDTOs;

namespace E_commerce_API.src.Application.DTOs.OrderItemDTOs
{
    public class OrderItemSummaryDTO
    {
        public int Id { get; set; }
        public ProductSummaryDTO ProductSummaryDTO { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
