using Ecommerce.Application.DTOs.ProductDTOs;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Mappers;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;
using static System.Net.Mime.MediaTypeNames;

namespace Ecommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ProductListDTO>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            var productListDTOs = products.Select(x => ProductMapper.ToListDTO(x));
            return productListDTOs;
        }
        public async Task<IEnumerable<ProductListDTO>> GetAllByCategoryIdAsync(Guid categoryId)
        {
            var products = await _productRepository.GetAllByCategoryIdAsync(categoryId);

            var productListDTOs = products.Select(x => ProductMapper.ToListDTO(x));
            return productListDTOs;
        }
        public async Task<ProductDetailsDTO> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                throw new KeyNotFoundException($"Product with Id: {id} was not found");

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> CreateAsync(ProductCreateDTO productCreate)
        {
            var product = new Product(
                new ProductName(productCreate.Name), 
                new ProductShortDescription(productCreate.ShortDescription), 
                new ProductLongDescription(productCreate.LongDescription),
                new Money(productCreate.Price),
                new Quantity(productCreate.Stock)
            );
            _productRepository.Add(product);
            await _unitOfWork.SaveChangesAsync();

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> UpdateAsync(Guid productId, ProductUpdateDTO productUpdate)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
                throw new KeyNotFoundException($"Product with Id: {productId} was not found");

            if (productUpdate.Name is not null)
                product.ChangeName(new ProductName(productUpdate.Name));
            if (productUpdate.ShortDescription is not null)
                product.ChangeShortDescription(new ProductShortDescription(productUpdate.ShortDescription));
            if (productUpdate.LongDescription is not null)
                product.ChangeLongDescription(new ProductLongDescription(productUpdate.LongDescription));
            if (productUpdate.Price is not null)
                product.ChangePrice(new Money(productUpdate.Price.Value));
            if (productUpdate.Stock is not null)
                product.ChangeStock(new Quantity(productUpdate.Stock.Value));

            await _unitOfWork.SaveChangesAsync();

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> AddCategoryAsync(Guid productId, Guid categoryId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (product is null)
                throw new KeyNotFoundException($"Product with Id: {productId} was not found");
            if(category is null)
                throw new KeyNotFoundException($"Category with Id: {categoryId} was not found");

            product.AddCategory(category);
            await _unitOfWork.SaveChangesAsync();

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> RemoveCategoryAsync(Guid productId, Guid categoryId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
                throw new KeyNotFoundException($"Product with Id: {productId} was not found");

            product.RemoveCategory(categoryId);
            await _unitOfWork.SaveChangesAsync();

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> AddImageAsync(Guid productId, ProductImageDTO image)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
                throw new KeyNotFoundException($"Product with Id: {productId} was not found");

            product.AddProductImage(new ProductImage(image.Url, image.Order));
            await _unitOfWork.SaveChangesAsync();

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> RemoveImageAsync(Guid productId, ProductImageDTO image)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
                throw new KeyNotFoundException($"Product with Id: {productId} was not found");

            product.RemoveProductImage(new ProductImage(image.Url, image.Order));
            await _unitOfWork.SaveChangesAsync();

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> ChangeImageOrderAsync(Guid productId, ProductImageDTO image, int newOrder)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
                throw new KeyNotFoundException($"Product with Id: {productId} was not found");

            product.ChangeOrderProductImage(new ProductImage(image.Url, image.Order), newOrder);
            await _unitOfWork.SaveChangesAsync();

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> IncreaseStockAsync(Guid productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
                throw new KeyNotFoundException($"Product with Id: {productId} was not found");

            product.IncreaseStock(new Quantity(quantity));
            await _unitOfWork.SaveChangesAsync();

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> DecreaseStockAsync(Guid productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
                throw new KeyNotFoundException($"Product with Id: {productId} was not found");

            product.DecreaseStock(new Quantity(quantity));
            await _unitOfWork.SaveChangesAsync();

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task DeleteAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                throw new KeyNotFoundException($"Product with Id: {id} was not found");

            _productRepository.Remove(product);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
