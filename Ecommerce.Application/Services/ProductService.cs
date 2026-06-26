using Ecommerce.Application.DTOs.ProductDTOs;
using Ecommerce.Application.Interfaces.Services;

namespace Ecommerce.Application.Services
{
    public class ProductService : IProductService
    {
        public Task<IEnumerable<ProductListDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<ProductListDTO>> GetAllByCategoryIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }
        public Task<ProductDetailsDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<ProductDetailsDTO> CreateAsync(ProductCreateDTO productCreate)
        {
            throw new NotImplementedException();
        }
        public Task<ProductDetailsDTO> UpdateAsync(int productId, ProductUpdateDTO productUpdate)
        {
            throw new NotImplementedException();
        }
        public Task<ProductDetailsDTO> AddCategoryAsync(int productId, int categoryId)
        {
            throw new NotImplementedException();
        }
        public Task<ProductDetailsDTO> RemoveCategoryAsync(int productId, int categoryId)
        {
            throw new NotImplementedException();
        }
        public Task<ProductDetailsDTO> AddImageAsync(int productId, ProductImageDTO image)
        {
            throw new NotImplementedException();
        }
        public Task<ProductDetailsDTO> RemoveImageAsync(int productId, ProductImageDTO image)
        {
            throw new NotImplementedException();
        }
        public Task<ProductDetailsDTO> ChangeImageOrderAsync(int productId, ProductImageDTO image, int newOrder)
        {
            throw new NotImplementedException();
        }
        public Task<ProductDetailsDTO> IncreaseStockAsync(int productId, int quantity)
        {
            throw new NotImplementedException();
        }
        public Task<ProductDetailsDTO> DecreaseStockAsync(int productId, int quantity)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
