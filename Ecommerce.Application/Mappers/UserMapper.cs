using Ecommerce.Application.DTOs.ShippingDTOs;
using Ecommerce.Application.DTOs.UserDTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Mappers
{
    internal static class UserMapper
    {
        public static UserListDTO ToListDTO(User user)
        {
            var userListDTO = new UserListDTO()
            {
                Id = user.Id,
                FullName = user.FullName.Value,
                Role = user.Role
            };
            return userListDTO;
        }
        public static UserSummaryDTO ToSummaryDTO(User user)
        {
            var userSummaryDTO = new UserSummaryDTO()
            {
                Id = user.Id,
                FullName = user.FullName.Value,
                PhoneNumber = user.PhoneNumber.Value,
                Email = user.Email.Value,
                
            };
            return userSummaryDTO;
        }
        public static UserDetailsDTO ToDetailsDTO(User user)
        {
            var userDetailsDTO = new UserDetailsDTO()
            {
                Id = user.Id,
                FullName = user.FullName.Value,
                PhoneNumber = user.PhoneNumber.Value,
                Email = user.Email.Value,
                Role = user.Role,
                AvatarImageUrl = user.AvatarImage?.Url ?? "",
                ShippingAddresses = user.ShippingAddresses.Select(x => ShippingAddressToDTO(x)).ToList()
            };
            return userDetailsDTO;
        }
        private static ShippingAddressDTO ShippingAddressToDTO(ShippingAddress address)
        {
            var shippingAddressDTO = new ShippingAddressDTO()
            {
                RecipientName = address.RecipientName.Value,
                PhoneNumber = address.PhoneNumber.Value,
                Neighborhood = address.Neighborhood,
                Street = address.Street,
                Number = address.Number,
                State = address.State,
                City = address.City,
                ZipCode = address.ZipCode
            };
            return shippingAddressDTO;
        }
    }
}
