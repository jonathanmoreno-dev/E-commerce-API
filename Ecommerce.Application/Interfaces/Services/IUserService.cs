using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.ShippingDTOs;
using Ecommerce.Application.DTOs.UserDTOs;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<UserListDTO>> GetAllAsync();
        public Task<IEnumerable<UserSummaryDTO>> GetAllAdminsAsync();
        public Task<IEnumerable<UserSummaryDTO>> GetAllStandardUsersAsync();
        public Task<UserDetailsDTO> GetByIdAsync(Guid id);
        public Task<UserDetailsDTO> GetCurrentAsync();
        public Task<UserDetailsDTO> RegisterAsync(RegisterRequestDTO userRegister);
        public Task<UserDetailsDTO> LoginAsync(LoginRequestDTO userLogin);
        public Task<UserDetailsDTO> UpdateAsync(Guid userId, UserUpdateDTO userUpdate);
        public Task<UserDetailsDTO> ChangePasswordAsync(Guid userId, ChangePasswordDTO password);
        public Task<UserDetailsDTO> AddShippingAddressAsync(Guid userId, ShippingAddressDTO shippingAddress);
        public Task<UserDetailsDTO> RemoveShippingAddressAsync(Guid userId, ShippingAddressDTO shippingAddress);
        public Task DeleteAsync(Guid id);
    }
}
