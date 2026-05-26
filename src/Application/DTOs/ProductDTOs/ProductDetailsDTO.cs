using E_commerce_API.src.Application.DTOs.CategoryDTOs;

namespace E_commerce_API.src.Application.DTOs.ProductDTOs
{
    public class ProductDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string ShortDescription { get; set; } = "";
        public string LongDescription { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public List<CategoryListDTO> CategoryListDTOs { get; set; } = new();
    }
}
