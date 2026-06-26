using Ecommerce.Application.DTOs.CartDTOs;
using Ecommerce.Application.DTOs.CartItemDTOs;
using Ecommerce.Application.Interfaces.Services;

namespace Ecommerce.Application.Services
{
    public class CartService : ICartService
    {
        public Task<IEnumerable<CartListDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<CartDetailsDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<CartDetailsDTO> GetCurrentUserCartAsync()
        {
            throw new NotImplementedException();
        }
        public Task<CartDetailsDTO> CreateAsync()
        {
            throw new NotImplementedException();
        }
        public Task<CartDetailsDTO> AddItemAsync(CartItemCreateDTO item)
        {
            throw new NotImplementedException();
        }
        public Task<CartDetailsDTO> RemoveItemAsync(int productId)
        {
            throw new NotImplementedException();
        }
        public Task<CartDetailsDTO> UpdateItemAsync(int cartId, CartItemUpdateDTO itemUpdate)
        {
            throw new NotImplementedException();
        }
        public Task<CartDetailsDTO> ClearAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
