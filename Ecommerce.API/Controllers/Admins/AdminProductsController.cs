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
        public async Task<ActionResult<ProductDetailsDTO>> GetById(Guid id)
        {
            var category = await _productService.GetByIdAsync(id);
            return Ok(category);
        }
        [HttpPost]
        public async Task<ActionResult<ProductDetailsDTO>> Create(ProductCreateDTO requestDTO)
        {
            var category = await _productService.CreateAsync(requestDTO);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<ProductDetailsDTO>> Update(Guid id, ProductUpdateDTO requestDTO)
        {
            var category = await _productService.UpdateAsync(id, requestDTO);
            return Ok(category);
        }
        [HttpPost("{productId:guid}/categories/{categoryId:guid}")]
        public async Task<ActionResult<ProductDetailsDTO>> AddCategory(Guid productId, Guid categoryId)
        {
            var product = await _productService.AddCategoryAsync(productId, categoryId);
            return Ok(product);
        }
        [HttpDelete("{productId:guid}/categories/{categoryId:guid}")]
        public async Task<ActionResult<ProductDetailsDTO>> RemoveCategory(Guid productId, Guid categoryId)
        {
            var product = await _productService.RemoveCategoryAsync(productId, categoryId);
            return Ok(product);
        }
        [HttpPost("{productId:guid}/image")]
        public async Task<ActionResult<ProductDetailsDTO>> AddImage(Guid productId, ProductImageDTO requestDTO)
        {
            var product = await _productService.AddImageAsync(productId, requestDTO);
            return Ok(product);
        }
        [HttpPatch("{productId:guid}/images/url")]
        public async Task<ActionResult<ProductDetailsDTO>> ChangeImageUrl(Guid productId, ChangeImageUrlDTO requestDTO)
        {
            var product = await _productService.ChangeImageUrlAsync(productId, requestDTO);
            return Ok(product);
        }
        [HttpPatch("{productId:guid}/images/reorder")]
        public async Task<ActionResult<ProductDetailsDTO>> ChangeImageOrder(Guid productId, ChangeImageOrderDTO requestDTO)
        {
            var product = await _productService.ChangeImageOrderAsync(productId, requestDTO);
            return Ok(product);
        }
        [HttpDelete("{productId:guid}/image")]
        public async Task<ActionResult<ProductDetailsDTO>> RemoveImage(Guid productId, ProductImageDTO requestDTO)
        {
            var product = await _productService.RemoveImageAsync(productId, requestDTO);
            return Ok(product);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}
