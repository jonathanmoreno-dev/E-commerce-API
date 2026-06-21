using Ecommerce.Domain.Enums;
using Ecommerce.Application.DTOs.CheckoutItemDTOs;

namespace Ecommerce.Application.DTOs.CheckoutDTOs
{
    public class CheckoutDetailsDTO
    {
        public int Id { get; set; }
        public string FullAddress { get; set; } = "";
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public decimal ShippingCost { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<CheckoutItemSummaryDTO> CheckoutItems { get; set; } = new();
    }
}
