using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.DTOs.RefundDTOs;
using Ecommerce.Application.Interfaces.Services;
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
        public async Task<ActionResult<IEnumerable<OrderListItemDTO>>> GetAllCurrentUserOrders()
        {
            var orders = await _orderService.GetAllCurrentUserOrdersAsync();
            return Ok(orders);
        }
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<OrderListItemDTO>>> GetAllCurrentUserOrdersByStatus(OrderStatus status)
        {
            var orders = await _orderService.GetAllCurrentUserOrdersByStatusAsync(status);
            return Ok(orders);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetById(Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);
            return Ok(order);
        }
        [HttpPatch("refund")]
        public async Task<ActionResult<OrderDetailsDTO>> RefundItem(RefundCreateDTO requestDTO)
        {
            var order = await _orderService.RefundItemAsync(requestDTO);
            return Ok(order);
        }
    }
}
