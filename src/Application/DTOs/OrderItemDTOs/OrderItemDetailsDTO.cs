using E_commerce_API.src.Application.DTOs.ProductDTOs;
using E_commerce_API.src.Application.DTOs.RefundDTOs;

namespace E_commerce_API.src.Application.DTOs.OrderItemDTOs
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
