using Ecommerce.Application.DTOs.CategoryDTOs;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategoryListDTO>> GetAllAsync();
        public Task<IEnumerable<CategoryListDTO>> GetAllByProductIdAsync(int productId);
        public Task<CategoryDetailsDTO> GetByIdAsync(int id);
        public Task<CategoryDetailsDTO> CreateAsync(CategoryCreateDTO categoryCreate);
        public Task<CategoryDetailsDTO> UpdateAsync(int categoryId, CategoryUpdateDTO categoryUpdate);
        public Task DeleteAsync(int id);
    }
}
