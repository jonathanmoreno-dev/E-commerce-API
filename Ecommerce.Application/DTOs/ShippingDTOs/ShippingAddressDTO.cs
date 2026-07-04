namespace Ecommerce.Application.DTOs.ShippingDTOs
{
    public class ShippingAddressDTO
    {
        public string RecipientName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Neighborhood { get; set; } = "";
        public string Street { get; set; } = "";
        public string Number { get; set; } = "";
        public string State { get; set; } = "";
        public string City { get; set; } = "";
        public string ZipCode { get; set; } = "";
    }
}
