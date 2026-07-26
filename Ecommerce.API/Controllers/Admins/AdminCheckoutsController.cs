using Ecommerce.Application.DTOs.CheckoutDTOs;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers.Admins
{
    [ApiController]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [Route("api/admin/checkouts")]
    public class AdminCheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        public AdminCheckoutsController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CheckoutSummaryDTO>>> GetAll([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var categories = await _checkoutService.GetAllActiveAsync(paginationParams, cancellationToken);
            return Ok(categories);
        }
        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<IEnumerable<CheckoutSummaryDTO>>> GetAllByUserId(Guid userId, [FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var categories = await _checkoutService.GetAllActiveByUserIdAsync(userId, paginationParams, cancellationToken);
            return Ok(categories);
        }
        [HttpPatch("{id:guid}/payment/authorize")]
        public async Task<IActionResult> AuthorizePayment(Guid id, CancellationToken cancellationToken)
        {
            await _checkoutService.AuthorizePaymentAsync(id, cancellationToken);
            return NoContent();
        }
        [HttpPatch("{id:guid}/payment/complete")]
        public async Task<IActionResult> CompletePayment(Guid id, CancellationToken cancellationToken)
        {
            await _checkoutService.CompletePaymentAsync(id, cancellationToken);
            return NoContent();
        }
        [HttpPatch("{id:guid}/payment/fail")]
        public async Task<IActionResult> FailPayment(Guid id, CancellationToken cancellationToken)
        {
            await _checkoutService.FailPaymentAsync(id, cancellationToken);
            return NoContent();
        }
        [HttpPatch("{id:guid}/payment/cancel")]
        public async Task<IActionResult> CancelPayment(Guid id, CancellationToken cancellationToken)
        {
            await _checkoutService.CancelPaymentAsync(id, cancellationToken);
            return NoContent();
        }
        [HttpPatch("{id:guid}/payment/abandon")]
        public async Task<IActionResult> AbandonPayment(Guid id, CancellationToken cancellationToken)
        {
            await _checkoutService.AbandonPaymentAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
