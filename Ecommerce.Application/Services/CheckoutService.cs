using Ecommerce.Application.DTOs.CheckoutDTOs;
using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Mappers;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IOrderService _orderService;
        private readonly IUnitOfWork _unitOfWork;
        public CheckoutService(ICheckoutRepository checkoutRepository, IProductRepository productRepository, ICurrentUserService currentUserService, IOrderService orderService, IUnitOfWork unitOfWork)
        {
            _checkoutRepository = checkoutRepository;
            _productRepository = productRepository;
            _currentUserService = currentUserService;
            _orderService = orderService;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<CheckoutSummaryDTO>> GetAllActiveAsync()
        {
            var checkouts = await _checkoutRepository.GetAllActiveAsync();

            var checkoutSummaryDTOs = checkouts.Select(x => CheckoutMapper.ToSummaryDTO(x));
            return checkoutSummaryDTOs;
        }
        public async Task<IEnumerable<CheckoutSummaryDTO>> GetAllActiveByUserIdAsync(Guid userId)
        {
            var checkouts = await _checkoutRepository.GetAllActiveByUserIdAsync(userId);

            var checkoutSummaryDTOs = checkouts.Select(x => CheckoutMapper.ToSummaryDTO(x));
            return checkoutSummaryDTOs;
        }
        public async Task<IEnumerable<CheckoutSummaryDTO>> GetAllCurrentUserCheckoutsActiveAsync()
        {
            var currentCheckouts = await _checkoutRepository.GetAllActiveByUserIdAsync(_currentUserService.UserId);

            var currentCheckoutSummaryDTOs = currentCheckouts.Select(x => CheckoutMapper.ToSummaryDTO(x));
            return currentCheckoutSummaryDTOs;
        }
        public async Task<CheckoutDetailsDTO> GetByIdAsync(Guid id)
        {
            var checkout = await _checkoutRepository.GetByIdAsync(id);
            if(checkout is null || _currentUserService.UserId == checkout.UserId)
                throw new KeyNotFoundException($"Checkout with Id: {id} was not found");

            var checkoutDetailsDTO = CheckoutMapper.ToDetailsDTO(checkout);
            return checkoutDetailsDTO;
        }
        public async Task<CheckoutDetailsDTO> CreateAsync(CheckoutCreateDTO checkoutCreate)
        {
            var shippingAddress = new ShippingAddress(
                new PersonName(checkoutCreate.ShippingAddressDTO.RecipientName),
                new PhoneNumber(checkoutCreate.ShippingAddressDTO.PhoneNumber),
                checkoutCreate.ShippingAddressDTO.Neighborhood,
                checkoutCreate.ShippingAddressDTO.Street,
                checkoutCreate.ShippingAddressDTO.Number,
                checkoutCreate.ShippingAddressDTO.State,
                checkoutCreate.ShippingAddressDTO.City,
                checkoutCreate.ShippingAddressDTO.ZipCode
            );
            var shippingCost = new Money(30); // Fixed Value
            var paymentMethod = checkoutCreate.PaymentMethod;
            
            var items = new List<(Guid, Money, Quantity)>();
            foreach (var itemDTO in checkoutCreate.CheckoutItemsToCreate)
            {
                var product = await _productRepository.GetByIdAsync(itemDTO.ProductId);
                if (product is null)
                    throw new KeyNotFoundException($"Product with Id: {itemDTO.ProductId} was not found");

                items.Add((product.Id, product.Price, new Quantity(itemDTO.Quantity)));
            }

            var checkout = new Checkout(_currentUserService.UserId, shippingAddress, shippingCost, paymentMethod, items);
            _checkoutRepository.Add(checkout);
            await _unitOfWork.SaveChangesAsync();

            var checkoutDetailsDTO = CheckoutMapper.ToDetailsDTO(checkout);
            return checkoutDetailsDTO;
        }
        public async Task<CheckoutDetailsDTO> UpdateAsync(Guid checkoutId, CheckoutUpdateDTO checkoutUpdate)
        {
            var checkout = await _checkoutRepository.GetByIdAsync(checkoutId);
            if (checkout is null || _currentUserService.UserId == checkout.UserId)
                throw new KeyNotFoundException($"Checkout with Id: {checkoutId} was not found");

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
            await _unitOfWork.SaveChangesAsync();

            var checkoutDetailsDTO = CheckoutMapper.ToDetailsDTO(checkout);
            return checkoutDetailsDTO;
        }
        public async Task CreatePaymentAsync(Guid checkoutId)
        {
            var checkout = await _checkoutRepository.GetByIdWithPaymentAttemptsAsync(checkoutId);
            if (checkout is null || _currentUserService.UserId == checkout.UserId)
                throw new KeyNotFoundException($"Checkout with Id: {checkoutId} was not found");

            checkout.CreatePayment();
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task AuthorizePaymentAsync(Guid checkoutId)
        {
            var checkout = await _checkoutRepository.GetByIdAsync(checkoutId);
            if (checkout is null)
                throw new KeyNotFoundException($"Checkout with Id: {checkoutId} was not found");

            checkout.AuthorizePayment();
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task CompletePaymentAsync(Guid checkoutId)
        {
            var checkout = await _checkoutRepository.GetByIdAsync(checkoutId);
            if (checkout is null)
                throw new KeyNotFoundException($"Checkout with Id: {checkoutId} was not found");

            checkout.CompletePayment();
            _orderService.CreateFromCheckoutAsync(checkout);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task FailPaymentAsync(Guid checkoutId)
        {
            var checkout = await _checkoutRepository.GetByIdAsync(checkoutId);
            if (checkout is null)
                throw new KeyNotFoundException($"Checkout with Id: {checkoutId} was not found");

            checkout.FailPayment();
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task CancelPaymentAsync(Guid checkoutId)
        {
            var checkout = await _checkoutRepository.GetByIdAsync(checkoutId);
            if (checkout is null)
                throw new KeyNotFoundException($"Checkout with Id: {checkoutId} was not found");

            checkout.CancelPayment();
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task AbandonPaymentAsync(Guid checkoutId)
        {
            var checkout = await _checkoutRepository.GetByIdAsync(checkoutId);
            if (checkout is null)
                throw new KeyNotFoundException($"Checkout with Id: {checkoutId} was not found");

            checkout.AbandonPayment();
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            var checkout = await _checkoutRepository.GetByIdAsync(id);
            if (checkout is null || _currentUserService.UserId == checkout.UserId)
                throw new KeyNotFoundException($"Checkout with Id: {id} was not found");

            _checkoutRepository.Remove(checkout);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
