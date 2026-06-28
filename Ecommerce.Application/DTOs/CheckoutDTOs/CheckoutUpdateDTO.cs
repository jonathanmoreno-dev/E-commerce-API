using Ecommerce.Application.DTOs.ShippingDTOs;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.DTOs.CheckoutDTOs
{
    public class CheckoutUpdateDTO
    {
        public ShippingAddressDTO? ShippingAddress { get; set; } 
        public PaymentMethod? PaymentMethod { get; set; }
    }
}
