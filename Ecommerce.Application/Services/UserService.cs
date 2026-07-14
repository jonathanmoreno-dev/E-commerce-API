using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.ShippingDTOs;
using Ecommerce.Application.DTOs.UserDTOs;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Mappers;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepository, ICurrentUserService currentUserService, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<UserListDTO>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            var userListDTOs = users.Select(x => UserMapper.ToListDTO(x));
            return userListDTOs;
        }
        public async Task<IEnumerable<UserSummaryDTO>> GetAllByRoleAsync(UserRole role)
        {
            var users = await _userRepository.GetAllByRoleAsync(role);

            var userSummaryDTOs = users.Select(x => UserMapper.ToSummaryDTO(x));
            return userSummaryDTOs;
        }
        public async Task<UserDetailsDTO> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                throw new KeyNotFoundException($"User with Id: {id} was not found");

            var userDetailsDTO = UserMapper.ToDetailsDTO(user);
            return userDetailsDTO;
        }
        public async Task<UserDetailsDTO> GetCurrentAsync()
        {
            var user = await _userRepository.GetByIdAsync(_currentUserService.UserId);
            if (user is null)
                throw new KeyNotFoundException($"User with Id: {_currentUserService.UserId} was not found");

            var userDetailsDTO = UserMapper.ToDetailsDTO(user);
            return userDetailsDTO;
        }
        public async Task<UserDetailsDTO> UpdateAsync(UserUpdateDTO userUpdate)
        {
            var user = await _userRepository.GetByIdAsync(_currentUserService.UserId);
            if (user is null)
                throw new KeyNotFoundException($"User with Id: {_currentUserService.UserId} was not found");

            if (userUpdate.FullName is not null)
                user.ChangeName(new PersonName(userUpdate.FullName));
            if (userUpdate.Email is not null)
                user.ChangeEmail(new Email(userUpdate.Email));
            if (userUpdate.PhoneNumber is not null)
                user.ChangePhoneNumber(new PhoneNumber(userUpdate.PhoneNumber));
            if (userUpdate.AvatarImageUrl is not null)
                user.ChangeAvatarImage(new AvatarImage(userUpdate.AvatarImageUrl));

            await _unitOfWork.SaveChangesAsync();

            var userDetailsDTO = UserMapper.ToDetailsDTO(user);
            return userDetailsDTO;
        }
        public async Task ChangePasswordAsync(ChangePasswordDTO password)
        {
            var user = await _userRepository.GetByIdAsync(_currentUserService.UserId);
            if (user is null)
                throw new KeyNotFoundException($"User with Id: {_currentUserService.UserId} was not found");

            if (!_passwordHasher.VerifyPassword(password.CurrentPassword, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            user.ChangePasswordHash(_passwordHasher.HashPassword(password.NewPassword));
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task ChangeRoleAsync(Guid id, UserRole role)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                throw new KeyNotFoundException($"User with Id: {id} was not found");

            user.ChangeRole(role);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<UserDetailsDTO> AddShippingAddressAsync(ShippingAddressDTO shippingAddress)
        {
            var user = await _userRepository.GetByIdAsync(_currentUserService.UserId);
            if (user is null)
                throw new KeyNotFoundException($"User with Id: {_currentUserService.UserId} was not found");

            user.AddShippingAddress(new ShippingAddress(
                new PersonName(shippingAddress.RecipientName),
                new PhoneNumber(shippingAddress.PhoneNumber),
                shippingAddress.Neighborhood,
                shippingAddress.Street,
                shippingAddress.Number,
                shippingAddress.State,
                shippingAddress.City,
                shippingAddress.ZipCode
            ));
            await _unitOfWork.SaveChangesAsync();

            var userDetailsDTO = UserMapper.ToDetailsDTO(user);
            return userDetailsDTO;
        }
        public async Task<UserDetailsDTO> RemoveShippingAddressAsync(ShippingAddressDTO shippingAddress)
        {
            var user = await _userRepository.GetByIdAsync(_currentUserService.UserId);
            if (user is null)
                throw new KeyNotFoundException($"User with Id: {_currentUserService.UserId} was not found");

            user.RemoveShippingAddress(new ShippingAddress(
                new PersonName(shippingAddress.RecipientName),
                new PhoneNumber(shippingAddress.PhoneNumber),
                shippingAddress.Neighborhood,
                shippingAddress.Street,
                shippingAddress.Number,
                shippingAddress.State,
                shippingAddress.City,
                shippingAddress.ZipCode
            ));
            await _unitOfWork.SaveChangesAsync();

            var userDetailsDTO = UserMapper.ToDetailsDTO(user);
            return userDetailsDTO;
        }
        public async Task DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                throw new KeyNotFoundException($"User with Id: {id} was not found");

            _userRepository.Remove(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
