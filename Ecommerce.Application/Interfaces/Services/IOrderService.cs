using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.DTOs.RefundDTOs;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderListItemDTO>> GetAllByUserIdAsync(int userId);
        public Task<IEnumerable<OrderListItemDTO>> GetAllCurrentUserOrdersAsync();
        public Task<IEnumerable<OrderListItemDTO>> GetAllCurrentUserOrdersByStatusAsync(OrderStatus status);
        public Task<OrderDetailsDTO> GetByIdAsync(int id);
        public Task<OrderDetailsDTO> CreateFromCheckoutAsync(int checkoutId);
        public Task<OrderDetailsDTO> RefundItemAsync(RefundCreateDTO refundCreate);
        public Task SetTrackingCodeAsync(int orderId, string trackingCode);
        public Task CancelAsync(int orderId);
        public Task ProcessShippingAsync(int orderId);
        public Task MarkAsShippedAsync(int orderId);
        public Task MarkAsInTransitAsync(int orderId);
        public Task MarkAsDeliveredAsync(int orderId);
        public Task MarkAsReturnedAsync(int orderId);
    }
}
