using Ecommerce.Application.DTOs.ProductDTOs;

namespace Ecommerce.Application.DTOs.OrderItemDTOs
{
    public class OrderItemSummaryDTO
    {
        public int Id { get; set; }
        public ProductSummaryDTO Product { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public int RefundedQuantity { get; set; }
        public decimal RefundedTotal { get; set; }
    }
}
