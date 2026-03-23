using E_commerce_API.Enums;

namespace E_commerce_API.Entities
{
    public class Shipping
    {
        public int ShippingId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public string RecipientName { get; set; } = "";
        public string Address { get; set; } = "";
        public string State { get; set; } = "";
        public string City { get; set; } = "";
        public string ZipCode { get; set; } = "";
        public decimal ShippingCost { get; set; }
        public string? TrackingCode { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public ShippingStatus Status { get; set; }
    }
}
