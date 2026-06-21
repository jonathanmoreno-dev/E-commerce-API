using Ecommerce.Application.DTOs.CategoryDTOs;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategoryListDTO>> GetAllCategoriesAsync();
        public Task<CategoryDetailsDTO> GetCategoryByIdAsync(int id);
        public Task<CategoryDetailsDTO> CreateCategoryAsync(CategoryCreateDTO categoryCreate);
        public Task<CategoryDetailsDTO> UpdateCategoryAsync(int categoryId, CategoryUpdateDTO categoryUpdate);
        public Task DeleteCategoryAsync(int id);
    }
}
