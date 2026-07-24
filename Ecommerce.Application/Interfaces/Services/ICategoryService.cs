using Ecommerce.Application.DTOs.CategoryDTOs;
using Ecommerce.Application.Pagination;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        public Task<PagedList<CategoryListDTO>> GetAllAsync(PaginationParams paginationParams);
        public Task<PagedList<CategoryListDTO>> GetAllByProductIdAsync(Guid productId, PaginationParams paginationParams);
        public Task<CategoryDetailsDTO> GetByIdAsync(Guid id);
        public Task<CategoryDetailsDTO> CreateAsync(CategoryCreateDTO categoryCreate);
        public Task<CategoryDetailsDTO> UpdateAsync(Guid categoryId, CategoryUpdateDTO categoryUpdate);
        public Task DeleteAsync(Guid id);
    }
}
