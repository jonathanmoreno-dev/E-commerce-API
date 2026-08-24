using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.DTOs.RefundDTOs;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface IOrderService
    {
        public Task<PagedList<OrderListItemDTO>> GetAllByUserIdAsync(Guid userId, PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<PagedList<OrderListItemDTO>> GetAllCurrentUserOrdersAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<PagedList<OrderListItemDTO>> GetAllCurrentUserOrdersByStatusAsync(OrderStatus status, PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<OrderDetailsDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public void CreateFromCheckout(Checkout checkout);
        public Task<OrderDetailsDTO> RefundItemAsync(RefundCreateDTO refundCreate, CancellationToken cancellationToken);
        public Task SetTrackingCodeAsync(Guid orderId, string trackingCode, CancellationToken cancellationToken);
        public Task CancelAsync(Guid orderId, CancellationToken cancellationToken);
        public Task MarkAsProcessingAsync(Guid orderId, CancellationToken cancellationToken);
        public Task MarkAsShippedAsync(Guid orderId, CancellationToken cancellationToken);
        public Task MarkAsInTransitAsync(Guid orderId, CancellationToken cancellationToken);
        public Task MarkAsDeliveredAsync(Guid orderId, CancellationToken cancellationToken);
        public Task MarkAsReturnedAsync(Guid orderId, CancellationToken cancellationToken);
    }
}
