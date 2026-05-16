using E_commerce_API.src.Domain.Enums;
using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class Shipping
    {
        public int ShippingId { get; private set; }
        public int OrderId { get; private set; }
        public ShippingAddress ShippingAddress { get; private set; } = null!;
        public Money ShippingCost { get; private set; } = null!;
        public string? TrackingCode { get; private set; }
        public DateTime? ShippedDate { get; private set; }
        public DateTime? DeliveredDate { get; private set; }
        public ShippingStatus Status { get; private set; }
        private Shipping() { }
        public Shipping(string recipientName, string phoneNumber, string neighborhood, string street, string number, string state, string city, string zipCode, decimal shippingCost)
        {
            ShippingAddress = new ShippingAddress(new PersonName(recipientName), new PhoneNumber(phoneNumber), neighborhood, street, number, state, city, zipCode);
            ChangeShippingCost(shippingCost);
            Status = ShippingStatus.Pending;
        }
        public void ChangeRecipientName(string recipientName)
        {
            ShippingAddress = ShippingAddress.WithRecipientName(new PersonName(recipientName));
        }
        public void ChangePhoneNumber(string phoneNumber)
        {
            ShippingAddress = ShippingAddress.WithPhoneNumber(new PhoneNumber(phoneNumber));
        }
        public void ChangeNeighborhood(string neighborhood)
        {
            ShippingAddress = ShippingAddress.WithNeighborhood(neighborhood);
        }
        public void ChangeStreet(string street)
        {
            ShippingAddress = ShippingAddress.WithStreet(street);
        }
        public void ChangeNumber(string number)
        {
            ShippingAddress = ShippingAddress.WithNumber(number);
        }
        public void ChangeState(string state)
        {
            ShippingAddress = ShippingAddress.WithState(state);
        }
        public void ChangeCity(string city)
        {
            ShippingAddress = ShippingAddress.WithCity(city);
        }
        public void ChangeZipCode(string zipCode)
        {
            ShippingAddress = ShippingAddress.WithZipCode(zipCode);
        }
        public void ChangeShippingCost(decimal shippingCost)
        {
            ShippingCost = new Money(shippingCost);
        }
        public void SetTrackingCode(string trackingCode)
        {
            if (string.IsNullOrWhiteSpace(trackingCode))
                throw new ArgumentException("Tracking code cannot be empty", nameof(trackingCode));

            TrackingCode = trackingCode;
        }
        public void MarkAsProcessing()
        {
            if (Status != ShippingStatus.Pending)
                throw new InvalidOperationException("Only pending shipping can be processing");

            Status = ShippingStatus.Processing;
        }
        public void MarkAsShipped()
        {
            if (Status != ShippingStatus.Processing)
                throw new InvalidOperationException("Only processing shipping can be shipped");

            Status = ShippingStatus.Shipped;
            ShippedDate = DateTime.UtcNow;
        }
        public void MarkAsInTransit()
        {
            if (Status != ShippingStatus.Shipped)
                throw new InvalidOperationException("Only shipped shipping can be in transit");

            Status = ShippingStatus.InTransit;
        }
        public void MarkAsDelivered()
        {
            if (Status != ShippingStatus.InTransit)
                throw new InvalidOperationException("Only shipping in transit can be delivered");

            Status = ShippingStatus.Delivered;
            DeliveredDate = DateTime.UtcNow;
        }
        public void MarkAsReturned()
        {
            Status = ShippingStatus.Returned;
        }
    }
}
