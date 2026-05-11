using E_commerce_API.src.Domain.Enums;
using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class Shipping
    {
        public int ShippingId { get; private set; }
        public int OrderId { get; private set; }
        public Order Order { get; private set; } = null!;
        public ShippingAddress Address { get; private set; } = null!;
        public Money ShippingCost { get; private set; } = null!;
        public string? TrackingCode { get; private set; }
        public DateTime? ShippedDate { get; private set; }
        public DateTime? DeliveredDate { get; private set; }
        public ShippingStatus Status { get; private set; }
    }
}
