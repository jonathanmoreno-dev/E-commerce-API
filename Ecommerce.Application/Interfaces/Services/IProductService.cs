using Ecommerce.Application.DTOs.ProductDTOs;
using Ecommerce.Application.Pagination;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface IProductService
    {
        public Task<PagedList<ProductListDTO>> GetAllAsync(PaginationParams paginationParams);
        public Task<PagedList<ProductListDTO>> GetAllByCategoryIdAsync(Guid categoryId, PaginationParams paginationParams);
        public Task<ProductDetailsDTO> GetByIdAsync(Guid id);
        public Task<ProductDetailsDTO> CreateAsync(ProductCreateDTO productCreate);
        public Task<ProductDetailsDTO> UpdateAsync(Guid productId, ProductUpdateDTO productUpdate);
        public Task<ProductDetailsDTO> AddCategoryAsync(Guid productId, Guid categoryId);
        public Task<ProductDetailsDTO> RemoveCategoryAsync(Guid productId, Guid categoryId);
        public Task<ProductDetailsDTO> AddImageAsync(Guid productId, ProductImageDTO image);
        public Task<ProductDetailsDTO> RemoveImageAsync(Guid productId, ProductImageDTO image);
        public Task<ProductDetailsDTO> ChangeImageUrlAsync(Guid productId, ChangeImageUrlDTO changeImage);
        public Task<ProductDetailsDTO> ChangeImageOrderAsync(Guid productId, ChangeImageOrderDTO changeImage);
        public Task<ProductDetailsDTO> IncreaseStockAsync(Guid productId, int quantity);
        public Task<ProductDetailsDTO> DecreaseStockAsync(Guid productId, int quantity);
        public Task DeleteAsync(Guid id);
    }
}
