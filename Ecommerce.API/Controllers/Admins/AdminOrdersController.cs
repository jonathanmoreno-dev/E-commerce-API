using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers.Admins
{
    [ApiController]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [Route("api/admin/orders")]
    public class AdminOrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public AdminOrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<IEnumerable<OrderListItemDTO>>> GetAllByUserId(Guid userId, [FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetAllByUserIdAsync(userId, paginationParams, cancellationToken);
            return Ok(orders);
        }
        [HttpPatch("{id:guid}/tracking-code")]
        public async Task<IActionResult> SetTrackingCode(Guid id, string trackingCode, CancellationToken cancellationToken)
        {
            await _orderService.SetTrackingCodeAsync(id, trackingCode, cancellationToken);
            return NoContent();
        }
        [HttpPatch("{id:guid}/processing")]
        public async Task<IActionResult> ProcessShipping(Guid id, CancellationToken cancellationToken)
        {
            await _orderService.MarkAsProcessingAsync(id, cancellationToken);
            return NoContent();
        }
        [HttpPatch("{id:guid}/shipped")]
        public async Task<IActionResult> MarkAsShipped(Guid id, CancellationToken cancellationToken)
        {
            await _orderService.MarkAsShippedAsync(id, cancellationToken);
            return NoContent();
        }
        [HttpPatch("{id:guid}/in-transit")]
        public async Task<IActionResult> MarkAsInTransit(Guid id, CancellationToken cancellationToken)
        {
            await _orderService.MarkAsInTransitAsync(id, cancellationToken);
            return NoContent();
        }
        [HttpPatch("{id:guid}/delivered")]
        public async Task<IActionResult> MarkAsDelivered(Guid id, CancellationToken cancellationToken)
        {
            await _orderService.MarkAsDeliveredAsync(id, cancellationToken);
            return NoContent();
        }
        [HttpPatch("{id:guid}/returned")]
        public async Task<IActionResult> MarkAsReturned(Guid id, CancellationToken cancellationToken)
        {
            await _orderService.MarkAsReturnedAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
