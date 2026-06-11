using E_commerce_API.src.Application.DTOs.CheckoutItemDTOs;
using E_commerce_API.src.Domain.Enums;

namespace E_commerce_API.src.Application.DTOs.CheckoutDTOs
{
    public class CheckoutDetailsDTO
    {
        public int Id { get; set; }
        public string FullAddress { get; set; } = "";
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public decimal ShippingCost { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<CheckoutItemListDTO> CheckoutItems { get; set; } = new();
    }
}
