using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.DTOs.RefundDTOs;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderListItemDTO>>> GetAllCurrentUserOrders([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetAllCurrentUserOrdersAsync(paginationParams, cancellationToken);
            return Ok(orders);
        }
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<OrderListItemDTO>>> GetAllCurrentUserOrdersByStatus(OrderStatus status, [FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetAllCurrentUserOrdersByStatusAsync(status, paginationParams, cancellationToken);
            return Ok(orders);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetByIdAsync(id, cancellationToken);
            return Ok(order);
        }
        [HttpPatch("refund")]
        public async Task<ActionResult<OrderDetailsDTO>> RefundItem(RefundCreateDTO requestDTO, CancellationToken cancellationToken)
        {
            var order = await _orderService.RefundItemAsync(requestDTO, cancellationToken);
            return Ok(order);
        }
    }
}
