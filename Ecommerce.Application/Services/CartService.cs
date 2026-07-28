using System.Threading;
using Ecommerce.Application.DTOs.CartDTOs;
using Ecommerce.Application.DTOs.CartItemDTOs;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Mappers;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        public CartService(ICartRepository cartRepository, IProductRepository productRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }
        public async Task<PagedList<CartListDTO>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var carts = await _cartRepository.GetAllAsync(paginationParams, cancellationToken);

            var cartListDTOs = carts.Select(x => CartMapper.ToListDTO(x));
            return cartListDTOs;
        }
        public async Task<CartDetailsDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByIdAsync(id, cancellationToken);
            if (cart is null)
                throw new NotFoundException("Cart", $"Cart with Id: {id} was not found");

            var cartDetailsDTO = CartMapper.ToDetailsDTO(cart);
            return cartDetailsDTO;
        }
        public async Task<CartDetailsDTO> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId, cancellationToken);
            if(cart is null)
                throw new NotFoundException("Cart", $"Cart with User Id: {userId} was not found");

            var cartDetailsDTO = CartMapper.ToDetailsDTO(cart);
            return cartDetailsDTO;
        }
        public async Task<CartDetailsDTO> GetCurrentUserCartAsync(CancellationToken cancellationToken)
        {
            var currentCart = await GetCurrentCartAsync(cancellationToken);

            var currentCartDetailsDTO = CartMapper.ToDetailsDTO(currentCart);
            return currentCartDetailsDTO;
        }
        public async Task<CartDetailsDTO> AddItemAsync(CartItemCreateDTO item, CancellationToken cancellationToken)
        {
            var currentCart = await GetCurrentCartAsync(cancellationToken);

            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product is null)
                throw new NotFoundException("Product", $"Product with Id: {item.ProductId} was not found");

            var quantity = new Quantity(item.Quantity);
            product.CheckAvailability(quantity);

            currentCart.AddItem(product.Id, product.Price, quantity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var currentCartDetailsDTO = CartMapper.ToDetailsDTO(currentCart);
            return currentCartDetailsDTO;
        }
        public async Task<CartDetailsDTO> RemoveItemAsync(Guid productId, CancellationToken cancellationToken)
        {
            var currentCart = await GetCurrentCartAsync(cancellationToken);

            currentCart.RemoveItem(productId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var currentCartDetailsDTO = CartMapper.ToDetailsDTO(currentCart);
            return currentCartDetailsDTO;
        }
        public async Task<CartDetailsDTO> UpdateItemAsync(CartItemUpdateDTO itemUpdate, CancellationToken cancellationToken)
        {
            var currentCart = await GetCurrentCartAsync(cancellationToken);

            var product = await _productRepository.GetByIdAsync(itemUpdate.ProductId, cancellationToken);
            if (product is null)
                throw new NotFoundException("Product", $"Product with Id: {itemUpdate.ProductId} was not found");

            var quantity = new Quantity(itemUpdate.Quantity);
            product.CheckAvailability(quantity);

            currentCart.ChangeItemQuantity(product.Id, quantity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var currentCartDetailsDTO = CartMapper.ToDetailsDTO(currentCart);
            return currentCartDetailsDTO;
        }
        public async Task<CartDetailsDTO> ClearAsync(CancellationToken cancellationToken)
        {
            var currentCart = await GetCurrentCartAsync(cancellationToken);

            currentCart.ClearItems();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var currentCartDetailsDTO = CartMapper.ToDetailsDTO(currentCart);
            return currentCartDetailsDTO;
        }
        private async Task<Cart> GetCurrentCartAsync(CancellationToken cancellationToken)
        {
            var currentCart = await _cartRepository.GetByUserIdAsync(_currentUserService.UserId, cancellationToken);
            if (currentCart is null)
                throw new NotFoundException("Cart", $"Cart with User Id: {_currentUserService.UserId} was not found");

            return currentCart;
        }
    }
}
