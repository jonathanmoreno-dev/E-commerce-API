using Ecommerce.Application.DTOs.CartDTOs;
using Ecommerce.Application.DTOs.CategoryDTOs;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers.Admins
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/admin/carts")]
    public class AdminCartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICategoryService _categoryService;
        public AdminCartsController(ICartService cartService, ICategoryService categoryService)
        {
            _cartService = cartService;
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartListDTO>>> GetAllCarts()
        {
            var carts = await _cartService.GetAllAsync();
            return Ok(carts);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CartDetailsDTO>> GetCartById(Guid id)
        {
            var cart = await _cartService.GetByIdAsync(id);
            return Ok(cart);
        }
        [HttpGet("users/{userId:guid}/cart")]
        public async Task<ActionResult<CartDetailsDTO>> GetCartByUserId(Guid userId)
        {
            var cart = await _cartService.GetByUserIdAsync(userId);
            return Ok(cart);
        }
    }
}
