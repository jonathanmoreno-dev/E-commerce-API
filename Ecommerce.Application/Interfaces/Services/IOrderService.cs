using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.DTOs.RefundDTOs;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface IOrderService
    {
        public Task<PagedList<OrderListItemDTO>> GetAllByUserIdAsync(Guid userId, PaginationParams paginationParams);
        public Task<PagedList<OrderListItemDTO>> GetAllCurrentUserOrdersAsync(PaginationParams paginationParams);
        public Task<PagedList<OrderListItemDTO>> GetAllCurrentUserOrdersByStatusAsync(OrderStatus status, PaginationParams paginationParams);
        public Task<OrderDetailsDTO> GetByIdAsync(Guid id);
        internal void CreateFromCheckout(Checkout checkout);
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
