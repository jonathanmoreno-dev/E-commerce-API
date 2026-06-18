using Ecommerce.Application.DTOs.ProductDTOs;

namespace Ecommerce.Application.DTOs.CategoryDTOs
{
    public class CategoryDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string CategoryImageUrl { get; set; } = "";
        public List<ProductListDTO> ProductListDTOs { get; set; } = null!;
    }
}
