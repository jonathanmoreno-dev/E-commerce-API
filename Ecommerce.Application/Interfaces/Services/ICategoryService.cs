using Ecommerce.Application.DTOs.CategoryDTOs;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategoryListDTO>> GetAllCategories();
        public Task<CategoryDetailsDTO> GetCategoryById(int id);
        public Task<CategoryDetailsDTO> CreateCategory(CategoryCreateDTO categoryCreate);
        public Task<CategoryDetailsDTO> UpdateCategory(CategoryUpdateDTO categoryUpdate);
        public Task RemoveCategory(int id);
    }
}
