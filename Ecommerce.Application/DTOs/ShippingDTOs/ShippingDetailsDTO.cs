using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.DTOs.ShippingDTOs
{
    public class ShippingDetailsDTO
    {
        public int Id { get; set; }
        public string RecipientName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string FullAddress { get; set; } = "";
        public string? TrackingCode { get; set; } 
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public ShippingStatus Status { get; set; }
    }
}
