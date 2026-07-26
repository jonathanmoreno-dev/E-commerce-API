using Ecommerce.Application.DTOs.CartDTOs;
using Ecommerce.Application.DTOs.CartItemDTOs;
using Ecommerce.Application.Pagination;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICartService
    {
        public Task<PagedList<CartListDTO>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<CartDetailsDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<CartDetailsDTO> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        public Task<CartDetailsDTO> GetCurrentUserCartAsync(CancellationToken cancellationToken);
        public Task<CartDetailsDTO> AddItemAsync(CartItemCreateDTO item, CancellationToken cancellationToken);
        public Task<CartDetailsDTO> RemoveItemAsync(Guid productId, CancellationToken cancellationToken);
        public Task<CartDetailsDTO> UpdateItemAsync(CartItemUpdateDTO itemUpdate, CancellationToken cancellationToken);
        public Task<CartDetailsDTO> ClearAsync(CancellationToken cancellationToken);
    }
}
