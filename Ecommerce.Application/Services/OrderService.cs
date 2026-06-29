using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.DTOs.RefundDTOs;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Mappers;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IOrderRepository orderRepository, ICheckoutRepository checkoutRepository, IUserService userService, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _checkoutRepository = checkoutRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<OrderListItemDTO>> GetAllByUserIdAsync(int userId)
        {
            var currentOrders = await _orderRepository.GetAllByUserIdAsync(userId);
            
            var currentOrderListItemDTOs = currentOrders.Select(x => OrderMapper.ToListItemDTO(x));
            return currentOrderListItemDTOs;
        }
        public async Task<IEnumerable<OrderListItemDTO>> GetAllCurrentUserOrdersAsync()
        {
            var currentUser = await _userService.GetCurrentAsync();
            var currentOrders = await _orderRepository.GetAllByUserIdAsync(currentUser.Id);

            var currentOrderListItemDTOs = currentOrders.Select(x => OrderMapper.ToListItemDTO(x));
            return currentOrderListItemDTOs;
        }
        public async Task<IEnumerable<OrderListItemDTO>> GetAllCurrentUserOrdersByStatusAsync(OrderStatus status)
        {
            var currentUser = await _userService.GetCurrentAsync();
            var orders = await _orderRepository.GetAllByUserIdAndStatusAsync(currentUser.Id, status);

            var currentOrderListItemDTOs = orders.Select(x => OrderMapper.ToListItemDTO(x));
            return currentOrderListItemDTOs;
        }
        public async Task<OrderDetailsDTO> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if(order is null)
                throw new KeyNotFoundException($"Order with Id: {id} was not found");

            var orderDetailsDTO = OrderMapper.ToDetailsDTO(order);
            return orderDetailsDTO;
        }
        public async Task<OrderDetailsDTO> CreateFromCheckoutAsync(int checkoutId)
        {
            var checkout = await _checkoutRepository.GetByIdAsync(checkoutId);
            if(checkout is null)
                throw new KeyNotFoundException($"Checkout with Id: {checkoutId} was not found");
            if (checkout.CompletedPayment is null)
                throw new InvalidOperationException("Checkout payment must be completed before creating an order");

            var items = checkout.CheckoutItems.Select(x => (x.ProductId, x.UnitPrice, x.Quantity));
            var order = new Order(checkout.UserId, checkout.ShippingAddress, checkout.ShippingCost, checkout.PaymentMethod, items, checkout.CompletedPayment.Amount);

            _orderRepository.Add(order);
            await _unitOfWork.SaveChangesAsync();

            var orderDetailsDTO = OrderMapper.ToDetailsDTO(order);
            return orderDetailsDTO;
        }
        public async Task<OrderDetailsDTO> RefundItemAsync(RefundCreateDTO refundCreate)
        {
            var order = await _orderRepository.GetByIdAsync(refundCreate.OrderId);
            if (order is null)
                throw new KeyNotFoundException($"Order with Id: {refundCreate.OrderId} was not found");

            order.RefundItem(refundCreate.OrderItemId, new Quantity(refundCreate.Quantity));
            await _unitOfWork.SaveChangesAsync();

            var orderDetailsDTO = OrderMapper.ToDetailsDTO(order);
            return orderDetailsDTO;
        }
        public async Task SetTrackingCodeAsync(int orderId, string trackingCode)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null)
                throw new KeyNotFoundException($"Order with Id: {orderId} was not found");

            order.SetTrackingCode(trackingCode);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task CancelAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null)
                throw new KeyNotFoundException($"Order with Id: {orderId} was not found");

            order.Cancel();
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task ProcessShippingAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null)
                throw new KeyNotFoundException($"Order with Id: {orderId} was not found");

            order.MarkAsProcessing();
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task MarkAsShippedAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null)
                throw new KeyNotFoundException($"Order with Id: {orderId} was not found");

            order.MarkAsShipped();
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task MarkAsInTransitAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null)
                throw new KeyNotFoundException($"Order with Id: {orderId} was not found");

            order.MarkAsInTransit();
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task MarkAsDeliveredAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null)
                throw new KeyNotFoundException($"Order with Id: {orderId} was not found");

            order.MarkAsDelivered();
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task MarkAsReturnedAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null)
                throw new KeyNotFoundException($"Order with Id: {orderId} was not found");

            order.MarkAsReturned();
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
