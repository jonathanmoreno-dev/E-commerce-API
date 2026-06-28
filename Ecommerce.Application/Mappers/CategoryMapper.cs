using Ecommerce.Application.DTOs.CategoryDTOs;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Mappers
{
    internal static class CategoryMapper
    {
        public static CategoryListDTO ToListDTO(Category category)
        {
            var categoryListDTO = new CategoryListDTO()
            {
                Id = category.Id,
                Name = category.Name.Value,
                CategoryImageUrl = category.CategoryImage?.Url ?? ""
            };
            return categoryListDTO;
        }
        public static CategoryDetailsDTO ToDetailsDTO(Category category)
        {
            var categoryDetailsDTO = new CategoryDetailsDTO()
            {
                Id = category.Id,
                Name = category.Name.Value,
                Description = category.Description.Value,
                CategoryImageUrl = category.CategoryImage?.Url ?? "",
                Products = category.Products.Select(x => ProductMapper.ToListDTO(x)).ToList()
            };
            return categoryDetailsDTO;
        }
    }
}
