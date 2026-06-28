using Ecommerce.Application.DTOs.CategoryDTOs;

namespace Ecommerce.Application.DTOs.ProductDTOs
{
    public class ProductDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string ShortDescription { get; set; } = "";
        public string LongDescription { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public List<ProductImageDTO> ProductImages { get; set; } = new();
        public List<CategoryListDTO> Categories { get; set; } = new();
    }
}
