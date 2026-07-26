using Ecommerce.Application.DTOs.CartDTOs;
using Ecommerce.Application.DTOs.CartItemDTOs;
using Ecommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpGet]
        public async Task<ActionResult<CartDetailsDTO>> GetCurrentUserCart(CancellationToken cancellationToken)
        {
            var cart = await _cartService.GetCurrentUserCartAsync(cancellationToken);
            return Ok(cart);
        }
        [HttpPost("items")]
        public async Task<ActionResult<CartDetailsDTO>> AddItem(CartItemCreateDTO requestDTO, CancellationToken cancellationToken)
        {
            var cart = await _cartService.AddItemAsync(requestDTO, cancellationToken);
            return Ok(cart);
        }
        [HttpDelete("items/{productId:guid}")]
        public async Task<ActionResult<CartDetailsDTO>> RemoveItem(Guid productId, CancellationToken cancellationToken)
        {
            var cart = await _cartService.RemoveItemAsync(productId, cancellationToken);
            return Ok(cart);
        }
        [HttpPatch("items")]
        public async Task<ActionResult<CartDetailsDTO>> UpdateItem(CartItemUpdateDTO requestDTO, CancellationToken cancellationToken)
        {
            var cart = await _cartService.UpdateItemAsync(requestDTO, cancellationToken);
            return Ok(cart);
        }
        [HttpDelete("items")]
        public async Task<ActionResult<CartDetailsDTO>> ClearItems(CancellationToken cancellationToken)
        {
            var cart = await _cartService.ClearAsync(cancellationToken);
            return Ok(cart);
        }
    }
}
