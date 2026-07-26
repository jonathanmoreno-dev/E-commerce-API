using Ecommerce.Application.DTOs.CategoryDTOs;
using Ecommerce.Application.DTOs.ProductDTOs;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers.Admins
{
    [ApiController]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [Route("api/admin/products")]
    public class AdminProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public AdminProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductDetailsDTO>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var category = await _productService.GetByIdAsync(id, cancellationToken);
            return Ok(category);
        }
        [HttpPost]
        public async Task<ActionResult<ProductDetailsDTO>> Create(ProductCreateDTO requestDTO, CancellationToken cancellationToken)
        {
            var category = await _productService.CreateAsync(requestDTO, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<ProductDetailsDTO>> Update(Guid id, ProductUpdateDTO requestDTO, CancellationToken cancellationToken)
        {
            var category = await _productService.UpdateAsync(id, requestDTO, cancellationToken);
            return Ok(category);
        }
        [HttpPost("{productId:guid}/categories/{categoryId:guid}")]
        public async Task<ActionResult<ProductDetailsDTO>> AddCategory(Guid productId, Guid categoryId, CancellationToken cancellationToken)
        {
            var product = await _productService.AddCategoryAsync(productId, categoryId, cancellationToken);
            return Ok(product);
        }
        [HttpDelete("{productId:guid}/categories/{categoryId:guid}")]
        public async Task<ActionResult<ProductDetailsDTO>> RemoveCategory(Guid productId, Guid categoryId, CancellationToken cancellationToken)
        {
            var product = await _productService.RemoveCategoryAsync(productId, categoryId, cancellationToken);
            return Ok(product);
        }
        [HttpPost("{productId:guid}/image")]
        public async Task<ActionResult<ProductDetailsDTO>> AddImage(Guid productId, ProductImageDTO requestDTO, CancellationToken cancellationToken)
        {
            var product = await _productService.AddImageAsync(productId, requestDTO, cancellationToken);
            return Ok(product);
        }
        [HttpPatch("{productId:guid}/images/url")]
        public async Task<ActionResult<ProductDetailsDTO>> ChangeImageUrl(Guid productId, ChangeImageUrlDTO requestDTO, CancellationToken cancellationToken)
        {
            var product = await _productService.ChangeImageUrlAsync(productId, requestDTO, cancellationToken);
            return Ok(product);
        }
        [HttpPatch("{productId:guid}/images/reorder")]
        public async Task<ActionResult<ProductDetailsDTO>> ChangeImageOrder(Guid productId, ChangeImageOrderDTO requestDTO, CancellationToken cancellationToken)
        {
            var product = await _productService.ChangeImageOrderAsync(productId, requestDTO, cancellationToken);
            return Ok(product);
        }
        [HttpDelete("{productId:guid}/image")]
        public async Task<ActionResult<ProductDetailsDTO>> RemoveImage(Guid productId, ProductImageDTO requestDTO, CancellationToken cancellationToken)
        {
            var product = await _productService.RemoveImageAsync(productId, requestDTO, cancellationToken);
            return Ok(product);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _productService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
