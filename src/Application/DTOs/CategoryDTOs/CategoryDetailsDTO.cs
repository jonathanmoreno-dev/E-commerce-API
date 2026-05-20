using E_commerce_API.src.Application.DTOs.ProductDTOs;

namespace E_commerce_API.src.Application.DTOs.CategoryDTOs
{
    public class CategoryDetailsDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public List<ProductListDTO> ProductListDTOs { get; set; } = null!;
    }
}
