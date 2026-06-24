using Ecommerce.Application.DTOs.ProductDTOs;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductListDTO>> GetAllAsync();
        public Task<IEnumerable<ProductListDTO>> GetAllByCategoryIdAsync(int categoryId);
        public Task<ProductDetailsDTO> GetByIdAsync(int id);
        public Task<ProductDetailsDTO> CreateAsync(ProductCreateDTO productCreate);
        public Task<ProductDetailsDTO> UpdateAsync(int productId, ProductUpdateDTO productUpdate);
        public Task<ProductDetailsDTO> AddCategoryAsync(int productId, int categoryId);
        public Task<ProductDetailsDTO> RemoveCategoryAsync(int productId, int categoryId);
        public Task<ProductDetailsDTO> AddImageAsync(int productId, ProductImageDTO image);
        public Task<ProductDetailsDTO> RemoveImageAsync(int productId, ProductImageDTO image);
        public Task<ProductDetailsDTO> ChangeImageOrderAsync(int productId, ProductImageDTO image, int newOrder);
        public Task<ProductDetailsDTO> IncreaseStockAsync(int productId, int quantity);
        public Task<ProductDetailsDTO> DecreaseStockAsync(int productId, int quantity);
        public Task DeleteAsync(int id);
    }
}
