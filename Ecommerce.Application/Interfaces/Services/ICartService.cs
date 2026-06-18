using Ecommerce.Application.DTOs.CartDTOs;
using Ecommerce.Application.DTOs.CartItemDTOs;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICartService
    {
        public Task<IEnumerable<CartListDTO>> GetAllCartsAsync();
        public Task<CartDetailsDTO> GetCartByIdAsync(int id);
        public Task<CartDetailsDTO> GetCurrentCartAsync();
        public Task<CartDetailsDTO> CreateCartAsync();
        public Task<CartDetailsDTO> AddItemAsync(CartItemCreateDTO item);
        public Task<CartDetailsDTO> RemoveItemAsync(int productId);
        public Task<CartDetailsDTO> UpdateItemQuantityAsync(CartItemUpdateDTO itemUpdate);
        public Task<CartDetailsDTO> ClearAsync(int id);
    }
}
