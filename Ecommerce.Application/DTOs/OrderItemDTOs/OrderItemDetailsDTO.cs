using Ecommerce.Application.DTOs.ProductDTOs;
using Ecommerce.Application.DTOs.RefundDTOs;

namespace Ecommerce.Application.DTOs.OrderItemDTOs
{
    public class OrderItemDetailsDTO
    {
        public int Id { get; set; }
        public ProductSummaryDTO ProductSummaryDTO { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public List<RefundListDTO> RefundListDTOs { get; set; } = new();
        public decimal RefundedTotal { get; set; }
    }
}
