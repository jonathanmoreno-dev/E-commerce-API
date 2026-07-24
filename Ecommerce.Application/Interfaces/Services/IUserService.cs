using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.ShippingDTOs;
using Ecommerce.Application.DTOs.UserDTOs;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface IUserService
    {
        public Task<PagedList<UserListDTO>> GetAllAsync(PaginationParams paginationParams);
        public Task<PagedList<UserSummaryDTO>> GetAllByRoleAsync(UserRole role, PaginationParams paginationParams);
        public Task<UserDetailsDTO> GetByIdAsync(Guid id);
        public Task<UserDetailsDTO> GetCurrentAsync();
        public Task<UserDetailsDTO> UpdateAsync(UserUpdateDTO userUpdate);
        public Task ChangePasswordAsync(ChangePasswordDTO password);
        public Task ChangeRoleAsync(Guid id, UserRole role);
        public Task<UserDetailsDTO> AddShippingAddressAsync(ShippingAddressDTO shippingAddress);
        public Task<UserDetailsDTO> RemoveShippingAddressAsync(ShippingAddressDTO shippingAddress);
        public Task DeleteAsync(Guid id);
    }
}
