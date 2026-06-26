using Ecommerce.Application.DTOs.ShippingDTOs;
using Ecommerce.Application.DTOs.UserDTOs;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<UserListDTO>> GetAllAsync();
        public Task<IEnumerable<UserSummaryDTO>> GetAllAdminsAsync();
        public Task<IEnumerable<UserSummaryDTO>> GetAllStandardUsersAsync();
        public Task<UserDetailsDTO> GetByIdAsync(int id);
        public Task<UserDetailsDTO> GetCurrentAsync();
        public Task<UserDetailsDTO> CreateAsync(UserCreateDTO userCreate);
        public Task<UserDetailsDTO> UpdateAsync(int userId, UserUpdateDTO userUpdate);
        public Task<UserDetailsDTO> ChangePasswordAsync(int userId, ChangePasswordDTO password);
        public Task<UserDetailsDTO> AddShippingAddressAsync(int userId, ShippingAddressDTO shippingAddress);
        public Task<UserDetailsDTO> RemoveShippingAddressAsync(int userId, ShippingAddressDTO shippingAddress);
        public Task DeleteAsync(int id);
    }
}
