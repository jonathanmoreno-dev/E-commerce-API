using E_commerce_API.src.Application.DTOs.ProductDTOs;

namespace E_commerce_API.src.Application.DTOs.CheckoutItemDTOs
{
    public class CheckoutItemListDTO
    {
        public int Id { get; set; }
        public ProductSummaryDTO ProductSummaryDTO { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
