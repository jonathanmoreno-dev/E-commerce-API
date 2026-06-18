using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.ShippingDTOs
{
    public class ShippingCreateDTO
    {
        [Required]
        public ShippingAddressDTO ShippingAddressDTO { get; set; } = null!;
        public decimal ShippingCost { get; set; }
    }
}
