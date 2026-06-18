using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.DTOs.ShippingDTOs
{
    public class ShippingSummaryDTO
    {
        public int Id { get; set; }
        public string RecipientName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string FullAddress { get; set; } = "";
        public ShippingStatus Status { get; set; }
    }
}
