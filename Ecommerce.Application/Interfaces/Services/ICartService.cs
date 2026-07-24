using Ecommerce.Application.DTOs.CartDTOs;
using Ecommerce.Application.DTOs.CartItemDTOs;
using Ecommerce.Application.Pagination;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICartService
    {
        public Task<PagedList<CartListDTO>> GetAllAsync(PaginationParams paginationParams);
        public Task<CartDetailsDTO> GetByIdAsync(Guid id);
        public Task<CartDetailsDTO> GetByUserIdAsync(Guid userId);
        public Task<CartDetailsDTO> GetCurrentUserCartAsync();
        public Task<CartDetailsDTO> AddItemAsync(CartItemCreateDTO item);
        public Task<CartDetailsDTO> RemoveItemAsync(Guid productId);
        public Task<CartDetailsDTO> UpdateItemAsync(CartItemUpdateDTO itemUpdate);
        public Task<CartDetailsDTO> ClearAsync();
    }
}
