using System.Threading;
using Ecommerce.Application.DTOs.CategoryDTOs;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Pagination;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;
using Moq;

namespace Ecommerce.Tests.Unit.Application.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

        [Fact]
        public async Task ShouldReturnAllCategories()
        {
            var categories = new PagedList<Category>(new[] { CreateCategory(), CreateCategory() }, 1, 10, 2);
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var cancellationToken = new CancellationToken();

            _categoryRepositoryMock.Setup(x => x.GetAllAsync(paginationParams, cancellationToken)).ReturnsAsync(categories);

            var service = CreateService();

            var result = await service.GetAllAsync(paginationParams, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            _categoryRepositoryMock.Verify(x => x.GetAllAsync(paginationParams, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldReturnCategoriesByProductId()
        {
            var productId = Guid.NewGuid();
            var categories = new PagedList<Category>(new[] { CreateCategory() }, 1, 10, 1);
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var cancellationToken = new CancellationToken();

            _categoryRepositoryMock
                .Setup(x => x.GetAllByProductIdAsync(productId, paginationParams, cancellationToken))
                .ReturnsAsync(categories);

            var service = CreateService();

            var result = await service.GetAllByProductIdAsync(productId, paginationParams, cancellationToken);

            Assert.NotNull(result);
            Assert.Single(result.Items);
            _categoryRepositoryMock.Verify(
                x => x.GetAllByProductIdAsync(productId, paginationParams, cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task ShouldReturnCategoryById()
        {
            var category = CreateCategory();
            var cancellationToken = new CancellationToken();

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(category.Id, cancellationToken)).ReturnsAsync(category);

            var service = CreateService();

            var result = await service.GetByIdAsync(category.Id, cancellationToken);

            Assert.NotNull(result);

            _categoryRepositoryMock.Verify(x => x.GetByIdAsync(category.Id, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenCategoryByIdDoesNotExist()
        {
            var categoryId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(categoryId, cancellationToken)).ReturnsAsync((Category?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(categoryId,cancellationToken));
        }

        [Fact]
        public async Task ShouldCreateCategory()
        {
            var request = new CategoryCreateDTO
            {
                Name = "Electronics",
                Description = "Electronic products"
            };
            var cancellationToken = new CancellationToken();
            Category? addedCategory = null;

            _categoryRepositoryMock
                .Setup(x => x.Add(It.IsAny<Category>()))
                .Callback<Category>(category => addedCategory = category);

            var service = CreateService();

            var result = await service.CreateAsync(request, cancellationToken);

            Assert.NotNull(result);
            Assert.NotNull(addedCategory);
            Assert.Equal(new CategoryName(request.Name), addedCategory.Name);
            Assert.Equal(new CategoryDescription(request.Description), addedCategory.Description);

            _categoryRepositoryMock.Verify(x => x.Add(It.IsAny<Category>()), Times.Once);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldUpdateCategory()
        {
            var category = CreateCategory();
            var request = new CategoryUpdateDTO
            {
                Name = "Updated category",
                Description = "Updated category description",
                CategoryImageUrl = "https://example.com/category.png"
            };
            var cancellationToken = new CancellationToken();

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(category.Id, cancellationToken)).ReturnsAsync(category);

            var service = CreateService();

            var result = await service.UpdateAsync(category.Id, request, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(new CategoryName(request.Name), category.Name);
            Assert.Equal(new CategoryDescription(request.Description), category.Description);
            Assert.Equal(new CategoryImage(request.CategoryImageUrl), category.CategoryImage);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldKeepCategoryValuesWhenUpdateFieldsAreNull()
        {
            var category = CreateCategory();
            var name = category.Name;
            var description = category.Description;
            var cancellationToken = new CancellationToken();
            var request = new CategoryUpdateDTO();

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(category.Id, cancellationToken)).ReturnsAsync(category);

            var service = CreateService();

            await service.UpdateAsync(category.Id, request, cancellationToken);

            Assert.Equal(name, category.Name);
            Assert.Equal(description, category.Description);
            Assert.Null(category.CategoryImage);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenUpdatingCategoryDoesNotExist()
        {
            var categoryId = Guid.NewGuid();
            var request = new CategoryUpdateDTO { Name = "Updated category" };
            var cancellationToken = new CancellationToken();

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(categoryId, cancellationToken)).ReturnsAsync((Category?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(categoryId,request,cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldDeleteCategory()
        {
            var category = CreateCategory();
            var cancellationToken = new CancellationToken();

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(category.Id, cancellationToken)).ReturnsAsync(category);

            var service = CreateService();

            await service.DeleteAsync(category.Id, cancellationToken);

            _categoryRepositoryMock.Verify(x => x.Remove(category), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenDeletingCategoryDoesNotExist()
        {
            var categoryId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(categoryId, cancellationToken)).ReturnsAsync((Category?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteAsync(categoryId,cancellationToken));

            _categoryRepositoryMock.Verify(x => x.Remove(It.IsAny<Category>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        private CategoryService CreateService()
        {
            return new CategoryService(_categoryRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        private static Category CreateCategory()
        {
            return new Category(new CategoryName("Electronics"), new CategoryDescription("Electronic products"));
        }
    }
}
