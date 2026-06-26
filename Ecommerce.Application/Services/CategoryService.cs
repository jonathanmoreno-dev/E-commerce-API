using Ecommerce.Application.DTOs.CategoryDTOs;
using Ecommerce.Application.Interfaces.Services;

namespace Ecommerce.Application.Services
{
    public class CategoryService : ICategoryService
    {
        public Task<IEnumerable<CategoryListDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<CategoryDetailsDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<CategoryDetailsDTO> CreateAsync(CategoryCreateDTO categoryCreate)
        {
            throw new NotImplementedException();
        }
        public Task<CategoryDetailsDTO> UpdateAsync(int categoryId, CategoryUpdateDTO categoryUpdate)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
