using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.DTOs.RefundDTOs;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Mappers;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IOrderRepository orderRepository, ICheckoutRepository checkoutRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _checkoutRepository = checkoutRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }
        public async Task<PagedList<OrderListItemDTO>> GetAllByUserIdAsync(Guid userId, PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var currentOrders = await _orderRepository.GetAllByUserIdAsync(userId, paginationParams, cancellationToken);
            
            var currentOrderListItemDTOs = currentOrders.Select(x => OrderMapper.ToListItemDTO(x));
            return currentOrderListItemDTOs;
        }
        public async Task<PagedList<OrderListItemDTO>> GetAllCurrentUserOrdersAsync(PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var currentOrders = await _orderRepository.GetAllByUserIdAsync(_currentUserService.UserId, paginationParams, cancellationToken);

            var currentOrderListItemDTOs = currentOrders.Select(x => OrderMapper.ToListItemDTO(x));
            return currentOrderListItemDTOs;
        }
        public async Task<PagedList<OrderListItemDTO>> GetAllCurrentUserOrdersByStatusAsync(OrderStatus status, PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllByUserIdAndStatusAsync(_currentUserService.UserId, status, paginationParams, cancellationToken);

            var currentOrderListItemDTOs = orders.Select(x => OrderMapper.ToListItemDTO(x));
            return currentOrderListItemDTOs;
        }
        public async Task<OrderDetailsDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdForDetailsAsync(id, cancellationToken);
            if(order is null || _currentUserService.UserId != order.UserId)
                throw new NotFoundException("Order", $"Order with Id: {id} was not found");

            var orderDetailsDTO = OrderMapper.ToDetailsDTO(order);
            return orderDetailsDTO;
        }
        public void CreateFromCheckout(Checkout checkout)
        {
            if (checkout.CompletedPayment is null)
                throw new InvalidOperationException("Checkout payment must be completed before creating an order");

            var items = checkout.CheckoutItems.Select(x => (x.ProductId, x.UnitPrice, x.Quantity));
            var order = new Order(
                checkout.UserId, 
                checkout.ShippingAddress, 
                checkout.ShippingCost, 
                checkout.PaymentMethod ?? throw new InvalidOperationException("Checkout must have a payment method"), 
                items, 
                checkout.CompletedPayment.Amount);

            _orderRepository.Add(order);
        }
        public async Task<OrderDetailsDTO> RefundItemAsync(RefundCreateDTO refundCreate, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdForDetailsAsync(refundCreate.OrderId, cancellationToken);
            if (order is null || _currentUserService.UserId != order.UserId)
                throw new NotFoundException("Order", $"Order with Id: {refundCreate.OrderId} was not found");

            order.RefundItem(refundCreate.OrderItemId, new Quantity(refundCreate.Quantity));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var orderDetailsDTO = OrderMapper.ToDetailsDTO(order);
            return orderDetailsDTO;
        }
        public async Task SetTrackingCodeAsync(Guid orderId, string trackingCode, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
            if (order is null)
                throw new NotFoundException("Order", $"Order with Id: {orderId} was not found");

            order.SetTrackingCode(trackingCode);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task CancelAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
            if (order is null)
                throw new NotFoundException("Order", $"Order with Id: {orderId} was not found");

            order.Cancel();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task MarkAsProcessingAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
            if (order is null)
                throw new NotFoundException("Order", $"Order with Id: {orderId} was not found");

            order.MarkAsProcessing();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task MarkAsShippedAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
            if (order is null)
                throw new NotFoundException("Order", $"Order with Id: {orderId} was not found");

            order.MarkAsShipped();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task MarkAsInTransitAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
            if (order is null)
                throw new NotFoundException("Order", $"Order with Id: {orderId} was not found");

            order.MarkAsInTransit();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task MarkAsDeliveredAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
            if (order is null)
                throw new NotFoundException("Order", $"Order with Id: {orderId} was not found");

            order.MarkAsDelivered();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task MarkAsReturnedAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
            if (order is null)
                throw new NotFoundException("Order", $"Order with Id: {orderId} was not found");

            order.MarkAsReturned();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
