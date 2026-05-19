using E_commerce_API.src.Application.DTOs.ProductDTOs;

namespace E_commerce_API.src.Application.DTOs.CartItemDTOs
{
    public class CartItemListDTO
    {
        public int CartItemId { get; set; }
        public ProductSummaryDTO ProductSummaryDTO { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }
}
