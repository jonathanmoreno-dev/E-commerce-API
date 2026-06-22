using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.DTOs.RefundDTOs;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderListItemDTO>> GetAllCurrentAsync();
        public Task<IEnumerable<OrderListItemDTO>> GetAllByUserIdAsync(int userId);
        public Task<IEnumerable<OrderListItemDTO>> GetAllByStatusAsync(OrderStatus status);
        public Task<OrderDetailsDTO> GetByIdAsync(int id);
        public Task<OrderDetailsDTO> CreateFromCheckoutAsync(int checkoutId);
        public Task<OrderDetailsDTO> RefundItemAsync(RefundCreateDTO refundCreate);
        public Task CancelOrderAsync(int orderId);
        public Task CreateShippingAsync(int orderId);
        public Task ProcessShippingAsync(int orderId);
        public Task ShipShippingAsync(int orderId);
        public Task ShippingInTransitAsync(int orderId);
        public Task DeliverShippingAsync(int orderId);
        public Task ReturnShippingAsync(int orderId);
    }
}
