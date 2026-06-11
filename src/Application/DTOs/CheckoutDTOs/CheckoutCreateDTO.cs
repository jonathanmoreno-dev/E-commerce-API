using E_commerce_API.src.Application.DTOs.CheckoutItemDTOs;
using E_commerce_API.src.Application.DTOs.ShippingDTOs;
using E_commerce_API.src.Domain.Enums;

namespace E_commerce_API.src.Application.DTOs.CheckoutDTOs
{
    public class CheckoutCreateDTO
    {
        public int UserId { get; set; }
        public ShippingAddressDTO ShippingAddressDTO { get; set; } = null!;
        public decimal ShippingCost { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<CheckoutItemCreateDTO> CheckoutItemCreateDTOs { get; set; } = new();
    }
}
