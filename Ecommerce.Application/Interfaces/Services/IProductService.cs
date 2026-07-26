using Ecommerce.Application.DTOs.ProductDTOs;
using Ecommerce.Application.Pagination;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface IProductService
    {
        public Task<PagedList<ProductListDTO>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<PagedList<ProductListDTO>> GetAllByCategoryIdAsync(Guid categoryId, PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<ProductDetailsDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<ProductDetailsDTO> CreateAsync(ProductCreateDTO productCreate, CancellationToken cancellationToken);
        public Task<ProductDetailsDTO> UpdateAsync(Guid productId, ProductUpdateDTO productUpdate, CancellationToken cancellationToken);
        public Task<ProductDetailsDTO> AddCategoryAsync(Guid productId, Guid categoryId, CancellationToken cancellationToken);
        public Task<ProductDetailsDTO> RemoveCategoryAsync(Guid productId, Guid categoryId, CancellationToken cancellationToken);
        public Task<ProductDetailsDTO> AddImageAsync(Guid productId, ProductImageDTO image, CancellationToken cancellationToken);
        public Task<ProductDetailsDTO> RemoveImageAsync(Guid productId, ProductImageDTO image, CancellationToken cancellationToken);
        public Task<ProductDetailsDTO> ChangeImageUrlAsync(Guid productId, ChangeImageUrlDTO changeImage, CancellationToken cancellationToken);
        public Task<ProductDetailsDTO> ChangeImageOrderAsync(Guid productId, ChangeImageOrderDTO changeImage, CancellationToken cancellationToken);
        public Task<ProductDetailsDTO> IncreaseStockAsync(Guid productId, int quantity, CancellationToken cancellationToken);
        public Task<ProductDetailsDTO> DecreaseStockAsync(Guid productId, int quantity, CancellationToken cancellationToken);
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
