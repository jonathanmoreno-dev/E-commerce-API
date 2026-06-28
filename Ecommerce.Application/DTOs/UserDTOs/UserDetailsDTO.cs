using Ecommerce.Application.DTOs.CartDTOs;
using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.DTOs.ShippingDTOs;

namespace Ecommerce.Application.DTOs.UserDTOs
{
    public class UserDetailsDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public bool IsAdmin { get; set; }
        public string AvatarImageUrl { get; set; } = "";
        public List<ShippingAddressDTO> ShippingAddresses { get; set; } = new();
    }
}
