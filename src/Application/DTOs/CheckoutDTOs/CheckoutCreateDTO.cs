using System.ComponentModel.DataAnnotations;
using E_commerce_API.src.Application.DTOs.CheckoutItemDTOs;
using E_commerce_API.src.Application.DTOs.ShippingDTOs;
using E_commerce_API.src.Domain.Enums;

namespace E_commerce_API.src.Application.DTOs.CheckoutDTOs
{
    public class CheckoutCreateDTO
    {
        [Required]
        public ShippingAddressDTO ShippingAddressDTO { get; set; } = null!;
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public List<CheckoutItemCreateDTO> CheckoutItemCreateDTOs { get; set; } = new();
    }
}
