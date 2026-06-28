using System.ComponentModel.DataAnnotations;
using Ecommerce.Domain.Enums;
using Ecommerce.Application.DTOs.CheckoutItemDTOs;
using Ecommerce.Application.DTOs.ShippingDTOs;

namespace Ecommerce.Application.DTOs.CheckoutDTOs
{
    public class CheckoutCreateDTO
    {
        [Required]
        public ShippingAddressDTO ShippingAddressDTO { get; set; } = null!;
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public List<CheckoutItemCreateDTO> CheckoutItemsToCreate { get; set; } = new();
    }
}
