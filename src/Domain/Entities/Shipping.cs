using E_commerce_API.src.Domain.Enums;
using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class Shipping
    {
        public int ShippingId { get; private set; }
        public int OrderId { get; private set; }
        public Order Order { get; private set; } = null!;
        public ShippingAddress ShippingAddress { get; private set; } = null!;
        public Money ShippingCost { get; private set; } = null!;
        public string? TrackingCode { get; private set; }
        public DateTime? ShippedDate { get; private set; }
        public DateTime? DeliveredDate { get; private set; }
        public ShippingStatus Status { get; private set; }

        private Shipping() { }
        public Shipping(string recipientName, string phoneNumber, string neighborhood, string street, string number, string state, string city, string zipCode, decimal shippingCost)
        {
            ChangeRecipientName(recipientName);
            ChangePhoneNumber(phoneNumber);
            ChangeNeighborhood(neighborhood);
            ChangeStreet(street);
            ChangeNumber(number);
            ChangeState(state);
            ChangeCity(city);
            ChangeZipCode(zipCode);
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
    }
}
