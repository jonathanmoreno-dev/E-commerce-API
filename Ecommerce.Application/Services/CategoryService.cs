using System.Threading;
using Ecommerce.Application.DTOs.CategoryDTOs;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Mappers;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<PagedList<CategoryListDTO>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync(paginationParams, cancellationToken);

            var categoryListDTO = categories.Select(x => CategoryMapper.ToListDTO(x));
            return categoryListDTO;
        }
        public async Task<PagedList<CategoryListDTO>> GetAllByProductIdAsync(Guid productId, PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllByProductIdAsync(productId, paginationParams, cancellationToken);

            var categoryListDTO = categories.Select(x => CategoryMapper.ToListDTO(x));
            return categoryListDTO;
        }
        public async Task<CategoryDetailsDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
            if(category is null)
                throw new NotFoundException("Category", $"Category with Id: {id} was not found");

            var categoryDetailsDTO = CategoryMapper.ToDetailsDTO(category);
            return categoryDetailsDTO;
        }
        public async Task<CategoryDetailsDTO> CreateAsync(CategoryCreateDTO categoryCreate, CancellationToken cancellationToken)
        {
            var category = new Category(new CategoryName(categoryCreate.Name), new CategoryDescription(categoryCreate.Description));
            _categoryRepository.Add(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var categoryDetailsDTO = CategoryMapper.ToDetailsDTO(category);
            return categoryDetailsDTO;
        }
        public async Task<CategoryDetailsDTO> UpdateAsync(Guid categoryId, CategoryUpdateDTO categoryUpdate, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
            if (category is null)
                throw new NotFoundException("Category", $"Category with Id: {categoryId} was not found");

            if(categoryUpdate.Name is not null)
                category.ChangeName(new CategoryName(categoryUpdate.Name));

            if(categoryUpdate.Description is not null)
                category.ChangeDescription(new CategoryDescription(categoryUpdate.Description));

            if (categoryUpdate.CategoryImageUrl is not null)
                category.ChangeCategoryImage(new CategoryImage(categoryUpdate.CategoryImageUrl));

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var categoryDetailsDTO = CategoryMapper.ToDetailsDTO(category);
            return categoryDetailsDTO;
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
            if (category is null)
                throw new NotFoundException("Category", $"Category with Id: {id} was not found");

            _categoryRepository.Remove(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
