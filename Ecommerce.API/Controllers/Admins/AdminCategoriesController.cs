using Ecommerce.Application.DTOs.CategoryDTOs;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers.Admins
{
    [ApiController]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [Route("api/admin/categories")]
    public class AdminCategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public AdminCategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryDetailsDTO>> GetById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(category);
        }
        [HttpPost]
        public async Task<ActionResult<CategoryDetailsDTO>> Create(CategoryCreateDTO requestDTO)
        {
            var category = await _categoryService.CreateAsync(requestDTO);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<CategoryDetailsDTO>> Update(Guid id, CategoryUpdateDTO requestDTO)
        {
            var category = await _categoryService.UpdateAsync(id, requestDTO);
            return Ok(category);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
