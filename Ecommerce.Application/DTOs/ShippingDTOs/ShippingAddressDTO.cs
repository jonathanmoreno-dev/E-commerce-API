namespace Ecommerce.Application.DTOs.ShippingDTOs
{
    public class ShippingAddressDTO
    {
        public string RecipientName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Neighborhood { get; } = "";
        public string Street { get; } = "";
        public string Number { get; } = "";
        public string State { get; } = "";
        public string City { get; } = "";
        public string ZipCode { get; } = "";
    }
}
