using System.Threading;
using Ecommerce.Application.DTOs.CategoryDTOs;
using Ecommerce.Application.DTOs.ProductDTOs;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Pagination;
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
        public async Task<ActionResult<IEnumerable<CategoryListDTO>>> GetAll([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllAsync(paginationParams, cancellationToken);
            return Ok(categories);
        }
        [HttpGet("{id:guid}/products")]
        public async Task<ActionResult<IEnumerable<ProductListDTO>>> GetAllProductsByCategoryId(Guid id, [FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllByCategoryIdAsync(id, paginationParams, cancellationToken);
            return Ok(products);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryDetailsDTO>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetByIdAsync(id, cancellationToken);
            return Ok(category);
        }
    }
}
