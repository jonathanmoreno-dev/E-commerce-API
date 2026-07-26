using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.ShippingDTOs;
using Ecommerce.Application.DTOs.UserDTOs;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface IUserService
    {
        public Task<PagedList<UserListDTO>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<PagedList<UserSummaryDTO>> GetAllByRoleAsync(UserRole role, PaginationParams paginationParams, CancellationToken cancellationToken);
        public Task<UserDetailsDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<UserDetailsDTO> GetCurrentAsync(CancellationToken cancellationToken);
        public Task<UserDetailsDTO> UpdateAsync(UserUpdateDTO userUpdate, CancellationToken cancellationToken);
        public Task ChangePasswordAsync(ChangePasswordDTO password, CancellationToken cancellationToken);
        public Task ChangeRoleAsync(Guid id, UserRole role, CancellationToken cancellationToken);
        public Task<UserDetailsDTO> AddShippingAddressAsync(ShippingAddressDTO shippingAddress, CancellationToken cancellationToken);
        public Task<UserDetailsDTO> RemoveShippingAddressAsync(ShippingAddressDTO shippingAddress, CancellationToken cancellationToken);
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
