using System.Net;
using Ecommerce.Application.DTOs.CheckoutDTOs;
using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Mappers;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IOrderService _orderService;
        private readonly IUnitOfWork _unitOfWork;
        public CheckoutService(ICheckoutRepository checkoutRepository, ICartRepository cartRepository, IUserRepository userRepository, ICurrentUserService currentUserService, IOrderService orderService, IUnitOfWork unitOfWork)
        {
            _checkoutRepository = checkoutRepository;
            _cartRepository = cartRepository;
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _orderService = orderService;
            _unitOfWork = unitOfWork;
        }
        public async Task<PagedList<CheckoutSummaryDTO>> GetAllActiveAsync(PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var checkouts = await _checkoutRepository.GetAllActiveWithPaymentAttemptsAsync(paginationParams, cancellationToken);

            var checkoutSummaryDTOs = checkouts.Select(x => CheckoutMapper.ToSummaryDTO(x));
            return checkoutSummaryDTOs;
        }
        public async Task<PagedList<CheckoutSummaryDTO>> GetAllActiveByUserIdAsync(Guid userId, PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var checkouts = await _checkoutRepository.GetAllActiveWithPaymentAttemptsByUserIdAsync(userId, paginationParams, cancellationToken);

            var checkoutSummaryDTOs = checkouts.Select(x => CheckoutMapper.ToSummaryDTO(x));
            return checkoutSummaryDTOs;
        }
        public async Task<PagedList<CheckoutSummaryDTO>> GetAllCurrentUserCheckoutsActiveAsync(PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var currentCheckouts = await _checkoutRepository.GetAllActiveWithPaymentAttemptsByUserIdAsync(_currentUserService.UserId, paginationParams, cancellationToken);

            var currentCheckoutSummaryDTOs = currentCheckouts.Select(x => CheckoutMapper.ToSummaryDTO(x));
            return currentCheckoutSummaryDTOs;
        }
        public async Task<CheckoutDetailsDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var checkout = await _checkoutRepository.GetByIdAsync(id, cancellationToken);
            if(checkout is null || _currentUserService.UserId != checkout.UserId)
                throw new NotFoundException("Checkout", $"Checkout with Id: {id} was not found");

            var checkoutDetailsDTO = CheckoutMapper.ToDetailsDTO(checkout);
            return checkoutDetailsDTO;
        }
        public async Task<CheckoutDetailsDTO> CreateAsync(CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_currentUserService.UserId, cancellationToken);
            var cart = await _cartRepository.GetByUserIdAsync(_currentUserService.UserId, cancellationToken);
            if (user is null)
                throw new NotFoundException("User", $"User with Id: {_currentUserService.UserId} was not found");
            if(cart is null)
                throw new NotFoundException("Cart", $"Cart with User Id: {_currentUserService.UserId} was not found");

            var address = user.GetDefaultShippingAddress();
            var shippingCost = new Money(30); // Fixed Value
            foreach (var item in cart.CartItems)
            {
                if (item.Product is null)
                    throw new NotFoundException("Product", $"Product with Id: {item.ProductId} was deleted");

                item.Product.ReserveStock(item.Quantity);
            }
            var items = cart.CartItems.Select(x => (x.ProductId,x.UnitPrice,x.Quantity)).ToList();
            var checkout = new Checkout(_currentUserService.UserId, address, shippingCost, items);
            _checkoutRepository.Add(checkout);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var checkoutDetailsDTO = CheckoutMapper.ToDetailsDTO(checkout);
            return checkoutDetailsDTO;
        }
        public async Task<CheckoutDetailsDTO> UpdateAsync(Guid checkoutId, CheckoutUpdateDTO checkoutUpdate, CancellationToken cancellationToken)
        {
            var checkout = await _checkoutRepository.GetByIdAsync(checkoutId, cancellationToken);
            if (checkout is null || _currentUserService.UserId != checkout.UserId)
                throw new NotFoundException("Checkout", $"Checkout with Id: {checkoutId} was not found");

            if (checkoutUpdate.PaymentMethod is not null)
                checkout.ChangePaymentMethod(checkoutUpdate.PaymentMethod.Value);
            if (checkoutUpdate.ShippingAddress is not null)
                checkout.ChangeShippingAddress(new ShippingAddress(
                new PersonName(checkoutUpdate.ShippingAddress.RecipientName),
                new PhoneNumber(checkoutUpdate.ShippingAddress.PhoneNumber),
                checkoutUpdate.ShippingAddress.Neighborhood,
                checkoutUpdate.ShippingAddress.Street,
                checkoutUpdate.ShippingAddress.Number,
                checkoutUpdate.ShippingAddress.State,
                checkoutUpdate.ShippingAddress.City,
                checkoutUpdate.ShippingAddress.ZipCode
            ));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var checkoutDetailsDTO = CheckoutMapper.ToDetailsDTO(checkout);
            return checkoutDetailsDTO;
        }
        public async Task ProcessExpiredCheckoutsAsync(CancellationToken cancellationToken)
        {
            var checkouts = await _checkoutRepository.GetAllExpiredNotProcessedAsync(cancellationToken);
            foreach (var checkout in checkouts)
            {
                foreach (var checkoutItem in checkout.CheckoutItems)
                {
                    checkoutItem.Product.CancelStockReservation(checkoutItem.Quantity);
                }
                checkout.MarkExpirationAsProcessed();
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task CreatePaymentAsync(Guid checkoutId, CancellationToken cancellationToken)
        {
            var checkout = await _checkoutRepository.GetByIdAsync(checkoutId, cancellationToken);
            var cart = await _cartRepository.GetByUserIdAsync(_currentUserService.UserId, cancellationToken);
            if (checkout is null || _currentUserService.UserId != checkout.UserId)
                throw new NotFoundException("Checkout", $"Checkout with Id: {checkoutId} was not found");
            if (cart is null)
                throw new NotFoundException("Cart", $"Cart with User Id: {_currentUserService.UserId} was not found");
            checkout.CreatePayment();
            cart.ClearItems();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task AuthorizePaymentAsync(Guid checkoutId, CancellationToken cancellationToken)
        {
            var checkout = await _checkoutRepository.GetByIdWithPaymentAttemptsAsync(checkoutId, cancellationToken);
            if (checkout is null)
                throw new NotFoundException("Checkout", $"Checkout with Id: {checkoutId} was not found");

            checkout.AuthorizePayment();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task CompletePaymentAsync(Guid checkoutId, CancellationToken cancellationToken)
        {
            var checkout = await _checkoutRepository.GetByIdWithPaymentAttemptsAsync(checkoutId, cancellationToken);
            if (checkout is null)
                throw new NotFoundException("Checkout", $"Checkout with Id: {checkoutId} was not found");

            foreach (var item in checkout.CheckoutItems)
            {
                item.Product.ConfirmStockReservation(item.Quantity);
            }
            checkout.CompletePayment();
            _orderService.CreateFromCheckout(checkout);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task FailPaymentAsync(Guid checkoutId, CancellationToken cancellationToken)
        {
            var checkout = await _checkoutRepository.GetByIdWithPaymentAttemptsAsync(checkoutId, cancellationToken);
            if (checkout is null)
                throw new NotFoundException("Checkout", $"Checkout with Id: {checkoutId} was not found");

            checkout.FailPayment();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task CancelPaymentAsync(Guid checkoutId, CancellationToken cancellationToken)
        {
            var checkout = await _checkoutRepository.GetByIdWithPaymentAttemptsAsync(checkoutId, cancellationToken);
            if (checkout is null)
                throw new NotFoundException("Checkout", $"Checkout with Id: {checkoutId} was not found");

            checkout.CancelPayment();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task AbandonPaymentAsync(Guid checkoutId, CancellationToken cancellationToken)
        {
            var checkout = await _checkoutRepository.GetByIdWithPaymentAttemptsAsync(checkoutId, cancellationToken);
            if (checkout is null)
                throw new NotFoundException("Checkout", $"Checkout with Id: {checkoutId} was not found");

            checkout.AbandonPayment();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var checkout = await _checkoutRepository.GetByIdAsync(id, cancellationToken);
            if (checkout is null || _currentUserService.UserId != checkout.UserId)
                throw new NotFoundException("Checkout", $"Checkout with Id: {id} was not found");

            _checkoutRepository.Remove(checkout);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
