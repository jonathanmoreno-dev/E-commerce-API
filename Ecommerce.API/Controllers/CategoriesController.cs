using Ecommerce.Application.DTOs.CategoryDTOs;
using Ecommerce.Application.DTOs.ProductDTOs;
using Ecommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        public CategoriesController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryListDTO>>> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }
        [HttpGet("{id:guid}/products")]
        public async Task<ActionResult<IEnumerable<ProductListDTO>>> GetAllProductsByCategoryId(Guid id)
        {
            var products = await _productService.GetAllByCategoryIdAsync(id);
            return Ok(products);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryDetailsDTO>> GetById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(category);
        }
    }
}
