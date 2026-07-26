using Ecommerce.Application.DTOs.CategoryDTOs;
using Ecommerce.Application.Pagination;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        public Task<PagedList<CategoryListDTO>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<PagedList<CategoryListDTO>> GetAllByProductIdAsync(Guid productId, PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<CategoryDetailsDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<CategoryDetailsDTO> CreateAsync(CategoryCreateDTO categoryCreate, CancellationToken cancellationToken);
        public Task<CategoryDetailsDTO> UpdateAsync(Guid categoryId, CategoryUpdateDTO categoryUpdate, CancellationToken cancellationToken);
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
