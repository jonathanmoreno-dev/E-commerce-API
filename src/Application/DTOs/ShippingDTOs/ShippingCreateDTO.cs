using System.ComponentModel.DataAnnotations;

namespace E_commerce_API.src.Application.DTOs.ShippingDTOs
{
    public class ShippingCreateDTO
    {
        [Required]
        public ShippingAddressDTO ShippingAddressDTO { get; set; } = null!;
        public decimal ShippingCost { get; set; }
    }
}
