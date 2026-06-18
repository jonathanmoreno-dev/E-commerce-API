using E_commerce_API.src.Application.DTOs.CartDTOs;
using E_commerce_API.src.Application.DTOs.CartItemDTOs;

namespace E_commerce_API.src.Application.Interfaces.Services
{
    public interface ICartService
    {
        public Task<IEnumerable<CartListDTO>> GetAllCartsAsync();
        public Task<CartDetailsDTO> GetCartByIdAsync(int id);
        public Task<CartDetailsDTO> GetCurrentCartAsync();
        public Task<CartDetailsDTO> CreateCartAsync();
        public Task<CartDetailsDTO> AddItemAsync(CartItemCreateDTO item);
        public Task<CartDetailsDTO> RemoveItemAsync(int productId);
        public Task<CartDetailsDTO> UpdateItemQuantityAsync(int productId, int quantity);
        public Task<CartDetailsDTO> ClearAsync(int id);
    }
}
