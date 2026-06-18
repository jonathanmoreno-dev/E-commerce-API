namespace Ecommerce.Application.DTOs.ProductDTOs
{
    public class ProductListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public string MainImageUrl { get; set; } = "";
    }
}
