using Ecommerce.Application.DTOs.ProductDTOs;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductListDTO>> GetAllAsync();
        public Task<IEnumerable<ProductListDTO>> GetAllByCategoryIdAsync(Guid categoryId);
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
