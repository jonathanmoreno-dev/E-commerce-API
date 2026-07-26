using Ecommerce.Application.DTOs.CategoryDTOs;
using Ecommerce.Application.DTOs.ProductDTOs;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Pagination;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductListDTO>>> GetAll([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllAsync(paginationParams, cancellationToken);
            return Ok(products);
        }
        [HttpGet("{id:guid}/categories")]
        public async Task<ActionResult<IEnumerable<CategoryListDTO>>> GetAllCategoriesByProductId(Guid id, [FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllByProductIdAsync(id, paginationParams, cancellationToken);
            return Ok(categories);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductDetailsDTO>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var category = await _productService.GetByIdAsync(id, cancellationToken);
            return Ok(category);
        }
    }
}
