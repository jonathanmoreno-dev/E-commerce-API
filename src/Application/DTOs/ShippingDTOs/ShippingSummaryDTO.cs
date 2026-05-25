using E_commerce_API.src.Domain.Enums;

namespace E_commerce_API.src.Application.DTOs.ShippingDTOs
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
