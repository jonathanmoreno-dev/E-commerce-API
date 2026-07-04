using Ecommerce.Application.DTOs.CategoryDTOs;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategoryListDTO>> GetAllAsync();
        public Task<IEnumerable<CategoryListDTO>> GetAllByProductIdAsync(Guid productId);
        public Task<CategoryDetailsDTO> GetByIdAsync(Guid id);
        public Task<CategoryDetailsDTO> CreateAsync(CategoryCreateDTO categoryCreate);
        public Task<CategoryDetailsDTO> UpdateAsync(Guid categoryId, CategoryUpdateDTO categoryUpdate);
        public Task DeleteAsync(Guid id);
    }
}
