using System.ComponentModel.DataAnnotations;
using E_commerce_API.src.Application.DTOs.OrderItemDTOs;
using E_commerce_API.src.Application.DTOs.ShippingDTOs;
using E_commerce_API.src.Domain.Enums;

namespace E_commerce_API.src.Application.DTOs.OrderDTOs
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
