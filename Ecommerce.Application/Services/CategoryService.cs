using Ecommerce.Application.DTOs.CategoryDTOs;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Mappers;
using Ecommerce.Domain.Entities;
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
        public async Task<IEnumerable<CategoryListDTO>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            var categoryListDTO = categories.Select(x => CategoryMapper.ToListDTO(x));
            return categoryListDTO;
        }
        public async Task<IEnumerable<CategoryListDTO>> GetAllByProductIdAsync(int productId)
        {
            var categories = await _categoryRepository.GetAllByProductIdAsync(productId);

            var categoryListDTO = categories.Select(x => CategoryMapper.ToListDTO(x));
            return categoryListDTO;
        }
        public async Task<CategoryDetailsDTO> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if(category is null)
                throw new KeyNotFoundException($"Category with Id: {id} was not found");

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
        public async Task<CategoryDetailsDTO> UpdateAsync(int categoryId, CategoryUpdateDTO categoryUpdate)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category is null)
                throw new KeyNotFoundException($"Category with Id: {categoryId} was not found");

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
        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
                throw new KeyNotFoundException($"Category with Id: {id} was not found");

            _categoryRepository.Remove(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
