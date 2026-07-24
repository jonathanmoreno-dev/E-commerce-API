using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.ValueObjects
{
    public record ShippingAddress
    {
        public PersonName RecipientName { get; } = null!;
        public PhoneNumber PhoneNumber { get; } = null!;
        public string Neighborhood { get; } = null!;
        public string Street { get; } = null!;
        public string Number { get; } = null!;
        public string State { get; } = null!;
        public string City { get; } = null!;
        public string ZipCode { get; } = null!;

        private ShippingAddress() { }
        public ShippingAddress(PersonName recipientName, PhoneNumber phoneNumber, string neighborhood, string street, string number, string state, string city, string zipCode)
        {
            RecipientName = recipientName;
            PhoneNumber = phoneNumber;
            ValidateNeighborhood(neighborhood);
            ValidateStreet(street);
            ValidateNumber(number);
            ValidateState(state);
            ValidateCity(city);
            ValidateZipCode(zipCode);
            Neighborhood = neighborhood;
            Street = street;
            Number = number;
            State = state;
            City = city;
            ZipCode = zipCode;
        }
        public ShippingAddress WithRecipientName(PersonName personName)
        {
            return new ShippingAddress(personName, PhoneNumber, Neighborhood, Street, Number, State, City, ZipCode);
        }
        public ShippingAddress WithPhoneNumber(PhoneNumber phoneNumber)
        {
            return new ShippingAddress(RecipientName, phoneNumber, Neighborhood, Street, Number, State, City, ZipCode);
        }
        public ShippingAddress WithNeighborhood(string neighborhood)
        {
            return new ShippingAddress(RecipientName, PhoneNumber, neighborhood, Street, Number, State, City, ZipCode);
        }
        public ShippingAddress WithStreet(string street)
        {
            return new ShippingAddress(RecipientName, PhoneNumber, Neighborhood, street, Number, State, City, ZipCode);
        }
        public ShippingAddress WithNumber(string number)
        {
            return new ShippingAddress(RecipientName, PhoneNumber, Neighborhood, Street, number, State, City, ZipCode);
        }
        public ShippingAddress WithState(string state)
        {
            return new ShippingAddress(RecipientName, PhoneNumber, Neighborhood, Street, Number, state, City, ZipCode);
        }
        public ShippingAddress WithCity(string city)
        {
            return new ShippingAddress(RecipientName, PhoneNumber, Neighborhood, Street, Number, State, city, ZipCode);
        }
        public ShippingAddress WithZipCode(string zipCode)
        {
            return new ShippingAddress(RecipientName, PhoneNumber, Neighborhood, Street, Number, State, City, zipCode);
        }
        private void ValidateNeighborhood(string neighborhood)
        {
            if (string.IsNullOrWhiteSpace(neighborhood))
                throw new DomainValidationException("Neighborhood cannot be empty");
            if(neighborhood.Length > 100)
                throw new DomainValidationException("Neighborhood cannot exceed 100 characters {nameof(neighborhood)}");
        }
        private void ValidateStreet(string street)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new DomainValidationException("Street cannot be empty");
            if (street.Length > 50)
                throw new DomainValidationException("Street cannot exceed 50 characters");
        }
        private void ValidateNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new DomainValidationException("Number cannot be empty");
            if (number.Length > 20)
                throw new DomainValidationException("Number cannot exceed 20 characters");
        }
        private void ValidateState(string state)
        {
            if (string.IsNullOrWhiteSpace(state))
                throw new DomainValidationException("State cannot be empty");
            if (state.Length > 50)
                throw new DomainValidationException("State cannot exceed 50 characters");
        }
        private void ValidateCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new DomainValidationException("City cannot be empty");
            if (city.Length > 100)
                throw new DomainValidationException("City cannot exceed 100 characters");
        }
        private void ValidateZipCode(string zipCode)
        {
            if (string.IsNullOrWhiteSpace(zipCode))
                throw new DomainValidationException("ZipCode cannot be empty");
            if (zipCode.Length > 20)
                throw new DomainValidationException("ZipCode cannot exceed 20 characters");
        }
    }
}
