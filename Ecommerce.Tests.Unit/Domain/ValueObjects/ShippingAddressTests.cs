using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.ValueObjects
{
    public class ShippingAddressTests
    {

        // ------ ShippingAddress ---------

        [Fact]
        public void ShouldBeEqualWhenShippingAddressesAreTheSame()
        {
            var shippingAddress1 = CreateShippingAddress();
            var shippingAddress2 = CreateShippingAddress();

            Assert.Equal(shippingAddress1, shippingAddress2);
        }
        [Fact]
        public void ShouldNotBeEqualWhenShippingAddressesAreDifferent()
        {
            var shippingAddress1 = CreateShippingAddress();
            var shippingAddress2 = CreateShippingAddress().WithNeighborhood("neighborhood");

            Assert.NotEqual(shippingAddress1, shippingAddress2);
        }
        [Fact]
        public void ShouldCreateValidShippingAddress()
        {
            var recipientNameExpected = new PersonName("Exemplo de Nome");
            var phoneNumberExpected = new PhoneNumber("+5549988887824");
            var neighborhoodExpected = "Palmeiras";
            var streetExpected = "Rodovia Sc-155";
            var numberExpected = "820";
            var stateExpected = "Paraná";
            var cityExpected = "Foz do Iguaçu";
            var zipCodeExpected = "43251-000";

            var shippingAddress = new ShippingAddress(
                recipientNameExpected,
                phoneNumberExpected,
                neighborhoodExpected,
                streetExpected,
                numberExpected,
                stateExpected,
                cityExpected,
                zipCodeExpected
            );

            Assert.Equal(recipientNameExpected.Value, shippingAddress.RecipientName.Value);
            Assert.Equal(phoneNumberExpected.Value, shippingAddress.PhoneNumber.Value);
            Assert.Equal(neighborhoodExpected, shippingAddress.Neighborhood);
            Assert.Equal(streetExpected, shippingAddress.Street);
            Assert.Equal(numberExpected, shippingAddress.Number);
            Assert.Equal(stateExpected, shippingAddress.State);
            Assert.Equal(cityExpected, shippingAddress.City);
            Assert.Equal("43251000", shippingAddress.ZipCode);
        }

        // ------ Neighborhood ---------

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenNeighborhoodIsNullOrWhiteSpace(string neighborhood)
        {
            Assert.Throws<DomainValidationException>(() => CreateShippingAddress().WithNeighborhood(neighborhood));
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenNeighborhoodIsMoreThan100Characters()
        {
            var neighborhood = new string('a', 101);

            Assert.Throws<DomainValidationException>(() => CreateShippingAddress().WithNeighborhood(neighborhood));
        }
        [Fact]
        public void ShouldCreateShippingAddressWithNeighborhoodExactly100Characters()
        {
            var neighborhood = new string('a', 100);

            var shippingAddress = CreateShippingAddress().WithNeighborhood(neighborhood);

            Assert.Equal(neighborhood, shippingAddress.Neighborhood);
        }
        [Fact]
        public void ShouldTrimNeighborhoodBeforeCreatingShippingAddress()
        {
            var neighborhood = new string('a', 43);

            var shippingAddress = CreateShippingAddress().WithNeighborhood($"     {neighborhood}     ");

            Assert.Equal(neighborhood, shippingAddress.Neighborhood);
        }

        // ------ Street ---------

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenStreetIsNullOrWhiteSpace(string street)
        {
            Assert.Throws<DomainValidationException>(() => CreateShippingAddress().WithStreet(street));
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenStreetIsMoreThan50Characters()
        {
            var street = new string('a', 51);

            Assert.Throws<DomainValidationException>(() => CreateShippingAddress().WithStreet(street));
        }
        [Fact]
        public void ShouldCreateShippingAddressWithStreetExactly50Characters()
        {
            var street = new string('a', 50);

            var shippingAddress = CreateShippingAddress().WithStreet(street);

            Assert.Equal(street, shippingAddress.Street);
        }
        [Fact]
        public void ShouldTrimStreetBeforeCreatingShippingAddress()
        {
            var street = new string('a', 30);

            var shippingAddress = CreateShippingAddress().WithStreet($"     {street}     ");

            Assert.Equal(street, shippingAddress.Street);
        }

        // ------ Number ---------

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenNumberIsNullOrWhiteSpace(string number)
        {
            Assert.Throws<DomainValidationException>(() => CreateShippingAddress().WithNumber(number));
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenNumberIsMoreThan20Characters()
        {
            var number = new string('a', 21);

            Assert.Throws<DomainValidationException>(() => CreateShippingAddress().WithNumber(number));
        }
        [Fact]
        public void ShouldCreateShippingAddressWithNumberExactly20Characters()
        {
            var number = new string('a', 20);

            var shippingAddress = CreateShippingAddress().WithNumber(number);

            Assert.Equal(number, shippingAddress.Number);
        }
        [Fact]
        public void ShouldTrimNumberBeforeCreatingShippingAddress()
        {
            var number = new string('a', 10);

            var shippingAddress = CreateShippingAddress().WithNumber($"     {number}     ");

            Assert.Equal(number, shippingAddress.Number);
        }
        
        // ------ State ---------

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenStateIsNullOrWhiteSpace(string state)
        {
            Assert.Throws<DomainValidationException>(() => CreateShippingAddress().WithState(state));
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenStateIsMoreThan50Characters()
        {
            var state = new string('a', 51);

            Assert.Throws<DomainValidationException>(() => CreateShippingAddress().WithState(state));
        }
        [Fact]
        public void ShouldCreateShippingAddressWithStateExactly50Characters()
        {
            var state = new string('a', 50);

            var shippingAddress = CreateShippingAddress().WithState(state);

            Assert.Equal(state, shippingAddress.State);
        }
        [Fact]
        public void ShouldTrimStateBeforeCreatingShippingAddress()
        {
            var state = new string('a', 30);

            var shippingAddress = CreateShippingAddress().WithState($"     {state}     ");

            Assert.Equal(state, shippingAddress.State);
        }

        // ------ City ---------

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenCityIsNullOrWhiteSpace(string city)
        {
            Assert.Throws<DomainValidationException>(() => CreateShippingAddress().WithCity(city));
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenCityIsMoreThan100Characters()
        {
            var city = new string('a', 101);

            Assert.Throws<DomainValidationException>(() => CreateShippingAddress().WithCity(city));
        }
        [Fact]
        public void ShouldCreateShippingAddressWithCityExactly100Characters()
        {
            var city = new string('a', 100);

            var shippingAddress = CreateShippingAddress().WithCity(city);

            Assert.Equal(city, shippingAddress.City);
        }
        [Fact]
        public void ShouldTrimCityBeforeCreatingShippingAddress()
        {
            var city = new string('a', 50);

            var shippingAddress = CreateShippingAddress().WithCity($"     {city}     ");

            Assert.Equal(city, shippingAddress.City);
        }

        // ------ ZipCode ---------

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenZipCodeIsNullOrWhiteSpace(string zipCode)
        {
            Assert.Throws<DomainValidationException>(() => CreateShippingAddress().WithZipCode(zipCode));
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenZipCodeIsMoreThan20Characters()
        {
            var zipCode = new string('a', 21);

            Assert.Throws<DomainValidationException>(() => CreateShippingAddress().WithZipCode(zipCode));
        }
        [Fact]
        public void ShouldCreateShippingAddressWithZipCodeExactly20Characters()
        {
            var zipCode = new string('a', 20);

            var shippingAddress = CreateShippingAddress().WithZipCode(zipCode);

            Assert.Equal(zipCode, shippingAddress.ZipCode);
        }
        [Fact]
        public void ShouldTrimZipCodeBeforeCreatingShippingAddress()
        {
            var zipCode = new string('a', 10);

            var shippingAddress = CreateShippingAddress().WithZipCode($"     {zipCode}     ");

            Assert.Equal(zipCode, shippingAddress.ZipCode);
        }
        [Fact]
        public void ShouldRemoveHyphenFromZipCodeBeforeCreatingShippingAddress()
        {
            var zipCode = "4389-4321";
            var zipCodeExpected = "43894321";

            var shippingAddress = CreateShippingAddress().WithZipCode(zipCode);

            Assert.Equal(zipCodeExpected, shippingAddress.ZipCode);
        }

        // ------ Helpers ---------

        private static ShippingAddress CreateShippingAddress()
        {
            return new ShippingAddress(
                new PersonName("Exemplo de Nome"),
                new PhoneNumber("+5549988887824"),
                "Palmeiras",
                "Rodovia Sc-155",
                "820",
                "Sc",
                "Foz do Iguaçu",
                "89760-000"
            );
        }
    }
}
