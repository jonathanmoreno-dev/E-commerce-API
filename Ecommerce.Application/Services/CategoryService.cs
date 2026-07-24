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
        public async Task<PagedList<CategoryListDTO>> GetAllAsync(PaginationParams paginationParams)
        {
            var categories = await _categoryRepository.GetAllAsync(paginationParams);

            var categoryListDTO = categories.Select(x => CategoryMapper.ToListDTO(x));
            return categoryListDTO;
        }
        public async Task<PagedList<CategoryListDTO>> GetAllByProductIdAsync(Guid productId, PaginationParams paginationParams)
        {
            var categories = await _categoryRepository.GetAllByProductIdAsync(productId, paginationParams);

            var categoryListDTO = categories.Select(x => CategoryMapper.ToListDTO(x));
            return categoryListDTO;
        }
        public async Task<CategoryDetailsDTO> GetByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if(category is null)
                throw new NotFoundException("Category", $"Category with Id: {id} was not found");

            var categoryDetailsDTO = CategoryMapper.ToDetailsDTO(category);
            return categoryDetailsDTO;
        }
        public async Task<CategoryDetailsDTO> CreateAsync(CategoryCreateDTO categoryCreate)
        {
            var category = new Category(new CategoryName(categoryCreate.Name), new CategoryDescription(categoryCreate.Description));
            _categoryRepository.Add(category);
            await _unitOfWork.SaveChangesAsync();

            var categoryDetailsDTO = CategoryMapper.ToDetailsDTO(category);
            return categoryDetailsDTO;
        }
        public async Task<CategoryDetailsDTO> UpdateAsync(Guid categoryId, CategoryUpdateDTO categoryUpdate)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category is null)
                throw new NotFoundException("Category", $"Category with Id: {categoryId} was not found");

            if(categoryUpdate.Name is not null)
                category.ChangeName(new CategoryName(categoryUpdate.Name));

            if(categoryUpdate.Description is not null)
                category.ChangeDescription(new CategoryDescription(categoryUpdate.Description));

            if (categoryUpdate.CategoryImageUrl is not null)
                category.ChangeCategoryImage(new CategoryImage(categoryUpdate.CategoryImageUrl));

            await _unitOfWork.SaveChangesAsync();
            var categoryDetailsDTO = CategoryMapper.ToDetailsDTO(category);
            return categoryDetailsDTO;
        }
        public async Task DeleteAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
                throw new NotFoundException("Category", $"Category with Id: {id} was not found");

            _categoryRepository.Remove(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
