using Ecommerce.Application.DTOs.ProductDTOs;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Mappers;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;
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
        public async Task<PagedList<ProductListDTO>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(paginationParams, cancellationToken);

            var productListDTOs = products.Select(x => ProductMapper.ToListDTO(x));
            return productListDTOs;
        }
        public async Task<PagedList<ProductListDTO>> GetAllByCategoryIdAsync(Guid categoryId, PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllByCategoryIdAsync(categoryId, paginationParams, cancellationToken);

            var productListDTOs = products.Select(x => ProductMapper.ToListDTO(x));
            return productListDTOs;
        }
        public async Task<ProductDetailsDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(id, cancellationToken);
            if (product is null)
                throw new NotFoundException("Product", $"Product with Id: {id} was not found");

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> CreateAsync(ProductCreateDTO productCreate, CancellationToken cancellationToken)
        {
            var product = new Product(
                new ProductName(productCreate.Name), 
                new ProductShortDescription(productCreate.ShortDescription), 
                new ProductLongDescription(productCreate.LongDescription),
                new Money(productCreate.Price),
                new Quantity(productCreate.Stock)
            );
            _productRepository.Add(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> UpdateAsync(Guid productId, ProductUpdateDTO productUpdate, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            if (product is null)
                throw new NotFoundException("Product", $"Product with Id: {productId} was not found");

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

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> AddCategoryAsync(Guid productId, Guid categoryId, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
            if (product is null)
                throw new NotFoundException("Product", $"Product with Id: {productId} was not found");
            if(category is null)
                throw new NotFoundException("Category", $"Category with Id: {categoryId} was not found");

            product.AddCategory(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> RemoveCategoryAsync(Guid productId, Guid categoryId, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            if (product is null)
                throw new NotFoundException("Product", $"Product with Id: {productId} was not found");

            product.RemoveCategory(categoryId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> AddImageAsync(Guid productId, ProductImageDTO image, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            if (product is null)
                throw new NotFoundException("Product", $"Product with Id: {productId} was not found");

            product.AddProductImage(new ProductImage(image.Url, image.Order));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> RemoveImageAsync(Guid productId, ProductImageDTO image, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            if (product is null)
                throw new NotFoundException("Product", $"Product with Id: {productId} was not found");

            product.RemoveProductImage(new ProductImage(image.Url, image.Order));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> ChangeImageUrlAsync(Guid productId, ChangeImageUrlDTO changeImage, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            if (product is null)
                throw new NotFoundException("Product", $"Product with Id: {productId} was not found");

            product.ChangeUrlProductImage(new ProductImage(changeImage.Image.Url, changeImage.Image.Order), changeImage.NewUrl);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task<ProductDetailsDTO> ChangeImageOrderAsync(Guid productId, ChangeImageOrderDTO changeImage, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            if (product is null)
                throw new NotFoundException("Product", $"Product with Id: {productId} was not found");

            product.ChangeOrderProductImage(new ProductImage(changeImage.Image.Url, changeImage.Image.Order), changeImage.NewOrder);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var productDetailsDTO = ProductMapper.ToDetailsDTO(product);
            return productDetailsDTO;
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(id, cancellationToken);
            if (product is null)
                throw new NotFoundException("Product", $"Product with Id: {id} was not found");

            _productRepository.Remove(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
