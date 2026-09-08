using System.Threading;
using Ecommerce.Application.DTOs.ProductDTOs;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Pagination;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;
using Moq;

namespace Ecommerce.Tests.Unit.Application.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock = new();
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

        [Fact]
        public async Task ShouldReturnAllProducts()
        {
            var products = new PagedList<Product>(
                new[] { CreateProduct(), CreateProduct() },
                1,
                10,
                2);
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetAllAsync(paginationParams, cancellationToken)).ReturnsAsync(products);

            var service = CreateService();

            var result = await service.GetAllAsync(paginationParams, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);

            _productRepositoryMock.Verify(x => x.GetAllAsync(paginationParams, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldReturnProductsByCategoryId()
        {
            var categoryId = Guid.NewGuid();
            var products = new PagedList<Product>(
                new[] { CreateProduct() },
                1,
                10,
                1);
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var cancellationToken = new CancellationToken();

            _productRepositoryMock
                .Setup(x => x.GetAllByCategoryIdAsync(categoryId, paginationParams, cancellationToken))
                .ReturnsAsync(products);

            var service = CreateService();

            var result = await service.GetAllByCategoryIdAsync(categoryId, paginationParams, cancellationToken);

            Assert.NotNull(result);
            Assert.Single(result.Items);

            _productRepositoryMock.Verify(
                x => x.GetAllByCategoryIdAsync(categoryId, paginationParams, cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task ShouldReturnProductById()
        {
            var product = CreateProduct();
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);

            var service = CreateService();

            var result = await service.GetByIdAsync(product.Id, cancellationToken);

            Assert.NotNull(result);

            _productRepositoryMock.Verify(x => x.GetByIdAsync(product.Id, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenProductByIdDoesNotExist()
        {
            var productId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId, cancellationToken)).ReturnsAsync((Product?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(productId, cancellationToken));
        }

        [Fact]
        public async Task ShouldCreateProduct()
        {
            var request = new ProductCreateDTO
            {
                Name = "New product",
                ShortDescription = "New short description",
                LongDescription = "New long product description",
                Price = 149.90m,
                Stock = 20
            };
            var cancellationToken = new CancellationToken();
            Product? addedProduct = null;

            _productRepositoryMock.Setup(x => x.Add(It.IsAny<Product>())).Callback<Product>(product => addedProduct = product);

            var service = CreateService();

            var result = await service.CreateAsync(request, cancellationToken);

            Assert.NotNull(result);
            Assert.NotNull(addedProduct);
            Assert.Equal(new ProductName(request.Name), addedProduct.Name);
            Assert.Equal(new ProductShortDescription(request.ShortDescription), addedProduct.ShortDescription);
            Assert.Equal(new ProductLongDescription(request.LongDescription), addedProduct.LongDescription);
            Assert.Equal(new Money(request.Price), addedProduct.Price);
            Assert.Equal(new Quantity(request.Stock), addedProduct.Stock);

            _productRepositoryMock.Verify(x => x.Add(It.IsAny<Product>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldUpdateProduct()
        {
            var product = CreateProduct();
            var request = new ProductUpdateDTO
            {
                Name = "Updated product",
                ShortDescription = "Updated short description",
                LongDescription = "Updated long product description",
                Price = 199.90m,
                Stock = 25
            };
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);

            var service = CreateService();

            var result = await service.UpdateAsync(product.Id, request, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(new ProductName(request.Name), product.Name);
            Assert.Equal(new ProductShortDescription(request.ShortDescription), product.ShortDescription);
            Assert.Equal(new ProductLongDescription(request.LongDescription), product.LongDescription);
            Assert.Equal(new Money(request.Price.Value), product.Price);
            Assert.Equal(new Quantity(request.Stock.Value), product.Stock);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldKeepProductValuesWhenUpdateFieldsAreNull()
        {
            var product = CreateProduct();
            var name = product.Name;
            var shortDescription = product.ShortDescription;
            var longDescription = product.LongDescription;
            var price = product.Price;
            var stock = product.Stock;
            var request = new ProductUpdateDTO();
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);

            var service = CreateService();

            await service.UpdateAsync(product.Id, request, cancellationToken);

            Assert.Equal(name, product.Name);
            Assert.Equal(shortDescription, product.ShortDescription);
            Assert.Equal(longDescription, product.LongDescription);
            Assert.Equal(price, product.Price);
            Assert.Equal(stock, product.Stock);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenUpdatingProductDoesNotExist()
        {
            var productId = Guid.NewGuid();
            var request = new ProductUpdateDTO { Name = "Updated product" };
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId, cancellationToken)).ReturnsAsync((Product?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(productId, request, cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldAddCategoryToProduct()
        {
            var product = CreateProduct();
            var category = CreateCategory();
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);
            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(category.Id, cancellationToken)).ReturnsAsync(category);

            var service = CreateService();

            var result = await service.AddCategoryAsync(product.Id, category.Id, cancellationToken);

            Assert.NotNull(result);
            Assert.Contains(category, product.Categories);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenAddingCategoryToMissingProduct()
        {
            var productId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId, cancellationToken)).ReturnsAsync((Product?)null);
            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(categoryId, cancellationToken)).ReturnsAsync(CreateCategory());

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.AddCategoryAsync(productId, categoryId, cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenAddingMissingCategoryToProduct()
        {
            var product = CreateProduct();
            var categoryId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);
            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(categoryId, cancellationToken)).ReturnsAsync((Category?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.AddCategoryAsync(product.Id, categoryId, cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowConflictExceptionWhenAddingDuplicateCategoryToProduct()
        {
            var product = CreateProduct();
            var category = CreateCategory();
            product.AddCategory(category);
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);
            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(category.Id, cancellationToken)).ReturnsAsync(category);

            var service = CreateService();

            await Assert.ThrowsAsync<ConflictException>(() => service.AddCategoryAsync(product.Id, category.Id, cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldRemoveCategoryFromProduct()
        {
            var product = CreateProduct();
            var category = CreateCategory();
            product.AddCategory(category);
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);

            var service = CreateService();

            var result = await service.RemoveCategoryAsync(product.Id, category.Id, cancellationToken);

            Assert.NotNull(result);
            Assert.Empty(product.Categories);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenRemovingCategoryFromMissingProduct()
        {
            var productId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId, cancellationToken)).ReturnsAsync((Product?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.RemoveCategoryAsync(
                productId,
                categoryId,
                cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenRemovingCategoryThatDoesNotExist()
        {
            var product = CreateProduct();
            var categoryId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.RemoveCategoryAsync(product.Id, categoryId, cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldAddImageToProduct()
        {
            var product = CreateProduct();
            var image = new ProductImageDTO
            {
                Url = "https://example.com/image.png",
                Order = 1
            };
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);

            var service = CreateService();

            var result = await service.AddImageAsync(product.Id, image, cancellationToken);

            Assert.NotNull(result);
            var addedImage = Assert.Single(product.ProductImages);
            Assert.Equal(image.Url, addedImage.Url);
            Assert.Equal(1, addedImage.Order);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldRemoveImageFromProduct()
        {
            var product = CreateProductWithImages();
            var image = new ProductImageDTO
            {
                Url = "https://example.com/first.png",
                Order = 1
            };
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);

            var service = CreateService();

            var result = await service.RemoveImageAsync(product.Id, image, cancellationToken);

            Assert.NotNull(result);
            var remainingImage = Assert.Single(product.ProductImages);
            Assert.Equal("https://example.com/second.png", remainingImage.Url);
            Assert.Equal(1, remainingImage.Order);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenRemovingImageThatDoesNotExist()
        {
            var product = CreateProduct();
            var image = new ProductImageDTO
            {
                Url = "https://example.com/image.png",
                Order = 1
            };
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.RemoveImageAsync(product.Id, image, cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldChangeProductImageUrl()
        {
            var product = CreateProductWithImages();
            var changeImage = new ChangeImageUrlDTO
            {
                Image = new ProductImageDTO
                {
                    Url = "https://example.com/first.png",
                    Order = 1
                },
                NewUrl = "https://example.com/updated.png"
            };
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);

            var service = CreateService();

            var result = await service.ChangeImageUrlAsync(product.Id, changeImage, cancellationToken);

            Assert.NotNull(result);
            Assert.Contains(product.ProductImages, x => x.Url == changeImage.NewUrl);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldChangeProductImageOrder()
        {
            var product = CreateProductWithImages();
            var changeImage = new ChangeImageOrderDTO
            {
                Image = new ProductImageDTO
                {
                    Url = "https://example.com/first.png",
                    Order = 1
                },
                NewOrder = 2
            };
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);

            var service = CreateService();

            var result = await service.ChangeImageOrderAsync(product.Id, changeImage, cancellationToken);

            Assert.NotNull(result);
            var images = product.ProductImages.ToList();

            Assert.Equal("https://example.com/second.png", images[0].Url);
            Assert.Equal(1, images[0].Order);
            Assert.Equal("https://example.com/first.png", images[1].Url);
            Assert.Equal(2, images[1].Order);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowDomainValidationExceptionWhenChangingImageOrderIsInvalid()
        {
            var product = CreateProductWithImages();
            var changeImage = new ChangeImageOrderDTO
            {
                Image = new ProductImageDTO
                {
                    Url = "https://example.com/first.png",
                    Order = 1
                },
                NewOrder = 3
            };
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);

            var service = CreateService();

            await Assert.ThrowsAsync<DomainValidationException>(() => service.ChangeImageOrderAsync(
product.Id,
                changeImage,
                cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldDeleteProduct()
        {
            var product = CreateProduct();
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);

            var service = CreateService();

            await service.DeleteAsync(product.Id, cancellationToken);

            _productRepositoryMock.Verify(x => x.Remove(product), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenDeletingProductDoesNotExist()
        {
            var productId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId, cancellationToken)).ReturnsAsync((Product?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteAsync(productId, cancellationToken));

            _productRepositoryMock.Verify(x => x.Remove(It.IsAny<Product>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        private ProductService CreateService()
        {
            return new ProductService(
                _productRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        private static Product CreateProduct()
        {
            return new Product(
                new ProductName("Product"),
                new ProductShortDescription("Short description"),
                new ProductLongDescription("Long product description"),
                new Money(99.90m),
                new Quantity(10));
        }

        private static Product CreateProductWithImages()
        {
            var product = CreateProduct();

            product.AddProductImage(new ProductImage("https://example.com/first.png", 1));
            product.AddProductImage(new ProductImage("https://example.com/second.png", 2));

            return product;
        }

        private static Category CreateCategory()
        {
            return new Category(new CategoryName("Category"), new CategoryDescription("Category description"));
        }
    }
}
