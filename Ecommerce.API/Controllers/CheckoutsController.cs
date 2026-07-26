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
        public async Task<ActionResult<IEnumerable<CheckoutSummaryDTO>>> GetAllCurrentUserCheckouts([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var checkouts = await _checkoutService.GetAllCurrentUserCheckoutsActiveAsync(paginationParams, cancellationToken);
            return Ok(checkouts);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CheckoutDetailsDTO>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var checkout = await _checkoutService.GetByIdAsync(id, cancellationToken);
            return Ok(checkout);
        }
        [HttpPost]
        public async Task<ActionResult<CheckoutDetailsDTO>> Create(CancellationToken cancellationToken)
        {
            var checkout = await _checkoutService.CreateAsync(cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = checkout.Id }, checkout);
        }
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<CheckoutDetailsDTO>> Update(Guid id, CheckoutUpdateDTO requestDTO, CancellationToken cancellationToken)
        {
            var checkout = await _checkoutService.UpdateAsync(id, requestDTO, cancellationToken);
            return Ok(checkout);
        }
        [HttpPost("{id:guid}/payment")]
        public async Task<IActionResult> CreatePayment(Guid id, CancellationToken cancellationToken)
        {
            await _checkoutService.CreatePaymentAsync(id, cancellationToken);
            return NoContent();
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _checkoutService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
