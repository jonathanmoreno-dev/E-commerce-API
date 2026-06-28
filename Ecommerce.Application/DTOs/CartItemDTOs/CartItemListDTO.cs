using Ecommerce.Application.DTOs.ProductDTOs;

namespace Ecommerce.Application.DTOs.CartItemDTOs
{
    public class CartItemListDTO
    {
        public int Id { get; set; }
        public ProductSummaryDTO Product { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }
}
