using Ecommerce.Application.DTOs.ProductDTOs;

namespace Ecommerce.Application.DTOs.CheckoutItemDTOs
{
    public class CheckoutItemListDTO
    {
        public int Id { get; set; }
        public ProductSummaryDTO ProductSummaryDTO { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
