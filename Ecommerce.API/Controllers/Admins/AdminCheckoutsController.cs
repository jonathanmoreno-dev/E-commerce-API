using Ecommerce.Application.DTOs.CheckoutDTOs;
using Ecommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers.Admins
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/admin/checkouts")]
    public class AdminCheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        public AdminCheckoutsController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CheckoutSummaryDTO>>> GetAll()
        {
            var categories = await _checkoutService.GetAllActiveAsync();
            return Ok(categories);
        }
        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<IEnumerable<CheckoutSummaryDTO>>> GetAllByUserId(Guid userId)
        {
            var categories = await _checkoutService.GetAllActiveByUserIdAsync(userId);
            return Ok(categories);
        }
        [HttpPatch("{id:guid}/payment/authorize")]
        public async Task<IActionResult> AuthorizePayment(Guid id)
        {
            await _checkoutService.AuthorizePaymentAsync(id);
            return NoContent();
        }
        [HttpPatch("{id:guid}/payment/complete")]
        public async Task<IActionResult> CompletePayment(Guid id)
        {
            await _checkoutService.CompletePaymentAsync(id);
            return NoContent();
        }
        [HttpPatch("{id:guid}/payment/fail")]
        public async Task<IActionResult> FailPayment(Guid id)
        {
            await _checkoutService.FailPaymentAsync(id);
            return NoContent();
        }
        [HttpPatch("{id:guid}/payment/cancel")]
        public async Task<IActionResult> CancelPayment(Guid id)
        {
            await _checkoutService.CancelPaymentAsync(id);
            return NoContent();
        }
        [HttpPatch("{id:guid}/payment/abandon")]
        public async Task<IActionResult> AbandonPayment(Guid id)
        {
            await _checkoutService.AbandonPaymentAsync(id);
            return NoContent();
        }
    }
}
