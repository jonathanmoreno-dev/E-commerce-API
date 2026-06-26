using Ecommerce.Application.DTOs.ShippingDTOs;
using Ecommerce.Application.DTOs.UserDTOs;
using Ecommerce.Application.Interfaces.Services;

namespace Ecommerce.Application.Services
{
    public class UserService : IUserService
    {
        public Task<IEnumerable<UserListDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<UserSummaryDTO>> GetAllAdminsAsync()
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<UserSummaryDTO>> GetAllStandardUsersAsync()
        {
            throw new NotImplementedException();
        }
        public Task<UserDetailsDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<UserDetailsDTO> GetCurrentAsync()
        {
            throw new NotImplementedException();
        }
        public Task<UserDetailsDTO> CreateAsync(UserCreateDTO userCreate)
        {
            throw new NotImplementedException();
        }
        public Task<UserDetailsDTO> UpdateAsync(int userId, UserUpdateDTO userUpdate)
        {
            throw new NotImplementedException();
        }
        public Task<UserDetailsDTO> ChangePasswordAsync(int userId, ChangePasswordDTO password)
        {
            throw new NotImplementedException();
        }
        public Task<UserDetailsDTO> AddShippingAddressAsync(int userId, ShippingAddressDTO shippingAddress)
        {
            throw new NotImplementedException();
        }
        public Task<UserDetailsDTO> RemoveShippingAddressAsync(int userId, ShippingAddressDTO shippingAddress)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
