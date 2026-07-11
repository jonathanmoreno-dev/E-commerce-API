using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.DTOs.RefundDTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderListItemDTO>> GetAllByUserIdAsync(Guid userId);
        public Task<IEnumerable<OrderListItemDTO>> GetAllCurrentUserOrdersAsync();
        public Task<IEnumerable<OrderListItemDTO>> GetAllCurrentUserOrdersByStatusAsync(OrderStatus status);
        public Task<OrderDetailsDTO> GetByIdAsync(Guid id);
        internal void CreateFromCheckoutAsync(Checkout checkout);
        public Task<OrderDetailsDTO> RefundItemAsync(RefundCreateDTO refundCreate);
        public Task SetTrackingCodeAsync(Guid orderId, string trackingCode);
        public Task CancelAsync(Guid orderId);
        public Task MarkAsProcessingAsync(Guid orderId);
        public Task MarkAsShippedAsync(Guid orderId);
        public Task MarkAsInTransitAsync(Guid orderId);
        public Task MarkAsDeliveredAsync(Guid orderId);
        public Task MarkAsReturnedAsync(Guid orderId);
    }
}
