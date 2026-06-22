using Ecommerce.Application.DTOs.CartDTOs;
using Ecommerce.Application.DTOs.CartItemDTOs;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICartService
    {
        public Task<IEnumerable<CartListDTO>> GetAllAsync();
        public Task<CartDetailsDTO> GetByIdAsync(int id);
        public Task<CartDetailsDTO> GetCurrentAsync();
        public Task<CartDetailsDTO> CreateAsync();
        public Task<CartDetailsDTO> AddItemAsync(CartItemCreateDTO item);
        public Task<CartDetailsDTO> RemoveItemAsync(int productId);
        public Task<CartDetailsDTO> UpdateItemAsync(CartItemUpdateDTO itemUpdate);
        public Task<CartDetailsDTO> ClearAsync(int id);
    }
}
