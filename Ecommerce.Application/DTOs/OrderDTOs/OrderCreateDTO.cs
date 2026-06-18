using System.ComponentModel.DataAnnotations;
using Ecommerce.Domain.Enums;
using Ecommerce.Application.DTOs.OrderItemDTOs;
using Ecommerce.Application.DTOs.ShippingDTOs;

namespace Ecommerce.Application.DTOs.OrderDTOs
{
    public class OrderCreateDTO
    {
        [Required]
        public ShippingAddressDTO ShippingAddressDTO { get; set; } = null!;
        public decimal ShippingCost { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public List<OrderItemCreateDTO> OrderItemCreateDTOs { get; set; } = new();
        public decimal TotalPaid { get; set; }
    }
}
