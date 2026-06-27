using Ecommerce.Application.DTOs.CartDTOs;
using Ecommerce.Application.DTOs.CartItemDTOs;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Mappers;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        public CartService(ICartRepository cartRepository, IProductRepository productRepository, IUserService userService, IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<CartListDTO>> GetAllAsync()
        {
            var carts = await _cartRepository.GetAllAsync();

            var cartListDTOs = carts.Select(x => CartMapper.ToListDTO(x)).ToList();
            return cartListDTOs;
        }
        public async Task<CartDetailsDTO> GetByIdAsync(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            if (cart is null)
                throw new KeyNotFoundException($"Cart with Id: {id} was not found");

            var cartDetailsDTO = CartMapper.ToDetailsDTO(cart);
            return cartDetailsDTO;
        }
        public async Task<CartDetailsDTO> GetByUserIdAsync(int userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if(cart is null)
                throw new KeyNotFoundException($"Cart with User Id: {userId} was not found");

            var cartDetailsDTO = CartMapper.ToDetailsDTO(cart);
            return cartDetailsDTO;
        }
        public async Task<CartDetailsDTO> GetCurrentUserCartAsync()
        {
            var currentCart = await GetCurrentCartAsync();

            var currentCartDetailsDTO = CartMapper.ToDetailsDTO(currentCart);
            return currentCartDetailsDTO;
        }
        public async Task<CartDetailsDTO> AddItemAsync(CartItemCreateDTO item)
        {
            var currentCart = await GetCurrentCartAsync();

            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product is null)
                throw new KeyNotFoundException($"Product with Id: {item.ProductId} was not found");

            currentCart.AddItem(product.Id, product.Price, new Quantity(item.Quantity));
            await _unitOfWork.SaveChangesAsync();

            var currentCartDetailsDTO = CartMapper.ToDetailsDTO(currentCart);
            return currentCartDetailsDTO;
        }
        public async Task<CartDetailsDTO> RemoveItemAsync(int productId)
        {
            var currentCart = await GetCurrentCartAsync();

            currentCart.RemoveItem(productId);
            await _unitOfWork.SaveChangesAsync();

            var currentCartDetailsDTO = CartMapper.ToDetailsDTO(currentCart);
            return currentCartDetailsDTO;
        }
        public async Task<CartDetailsDTO> UpdateItemAsync(CartItemUpdateDTO itemUpdate)
        {
            var currentCart = await GetCurrentCartAsync();

            currentCart.ChangeItemQuantity(itemUpdate.ProductId, new Quantity(itemUpdate.Quantity));
            await _unitOfWork.SaveChangesAsync();

            var currentCartDetailsDTO = CartMapper.ToDetailsDTO(currentCart);
            return currentCartDetailsDTO;
        }
        public async Task<CartDetailsDTO> ClearAsync()
        {
            var currentCart = await GetCurrentCartAsync();

            currentCart.ClearItems();
            await _unitOfWork.SaveChangesAsync();

            var currentCartDetailsDTO = CartMapper.ToDetailsDTO(currentCart);
            return currentCartDetailsDTO;
        }
        private async Task<Cart> GetCurrentCartAsync()
        {
            var currentUser = await _userService.GetCurrentAsync();
            var currentCart = await _cartRepository.GetByUserIdAsync(currentUser.Id);
            if (currentCart is null)
                throw new KeyNotFoundException($"Cart with User Id: {currentUser.Id} was not found");

            return currentCart;
        }
    }
}
