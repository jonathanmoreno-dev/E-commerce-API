using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.DTOs.RefundDTOs;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        public Task<IEnumerable<OrderListItemDTO>> GetAllCurrentUserOrdersAsync()
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<OrderListItemDTO>> GetAllByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<OrderListItemDTO>> GetAllByStatusAsync(OrderStatus status)
        {
            throw new NotImplementedException();
        }
        public Task<OrderDetailsDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<OrderDetailsDTO> CreateFromCheckoutAsync(int checkoutId)
        {
            throw new NotImplementedException();
        }
        public Task<OrderDetailsDTO> RefundItemAsync(RefundCreateDTO refundCreate)
        {
            throw new NotImplementedException();
        }
        public Task SetTrackingCodeAsync(int orderId, string trackingCode)
        {
            throw new NotImplementedException();
        }
        public Task CancelAsync(int orderId)
        {
            throw new NotImplementedException();
        }
        public Task ProcessShippingAsync(int orderId)
        {
            throw new NotImplementedException();
        }
        public Task MarkAsShippedAsync(int orderId)
        {
            throw new NotImplementedException();
        }
        public Task MarkAsInTransitAsync(int orderId)
        {
            throw new NotImplementedException();
        }
        public Task MarkAsDeliveredAsync(int orderId)
        {
            throw new NotImplementedException();
        }
        public Task MarkAsReturnedAsync(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
