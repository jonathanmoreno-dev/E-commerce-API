using Ecommerce.Application.DTOs.CheckoutDTOs;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        public CheckoutsController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CheckoutSummaryDTO>>> GetAllCurrentUserCheckouts([FromQuery] PaginationParams paginationParams)
        {
            var checkouts = await _checkoutService.GetAllCurrentUserCheckoutsActiveAsync(paginationParams);
            return Ok(checkouts);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CheckoutDetailsDTO>> GetById(Guid id)
        {
            var checkout = await _checkoutService.GetByIdAsync(id);
            return Ok(checkout);
        }
        [HttpPost]
        public async Task<ActionResult<CheckoutDetailsDTO>> Create()
        {
            var checkout = await _checkoutService.CreateAsync();
            return CreatedAtAction(nameof(GetById), new { id = checkout.Id }, checkout);
        }
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<CheckoutDetailsDTO>> Update(Guid id, CheckoutUpdateDTO requestDTO)
        {
            var checkout = await _checkoutService.UpdateAsync(id, requestDTO);
            return Ok(checkout);
        }
        [HttpPost("{id:guid}/payment")]
        public async Task<IActionResult> CreatePayment(Guid id)
        {
            await _checkoutService.CreatePaymentAsync(id);
            return NoContent();
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _checkoutService.DeleteAsync(id);
            return NoContent();
        }
    }
}
