using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecommerce.Tests.Unit.Domain.ValueObjects
{
    public class PhoneNumberTests
    {
        private const string MainPhoneNumber = "+5549988887824";
        [Fact]
        public void ShouldBeEqualWhenPhoneNumbersAreTheSame()
        {
            var number = MainPhoneNumber;

            var phoneNumber1 = new PhoneNumber(number);
            var phoneNumber2 = new PhoneNumber(number);

            Assert.Equal(phoneNumber1, phoneNumber2);
        }
        [Fact]
        public void ShouldNotBeEqualWhenPhoneNumbersAreDifferent()
        {
            var number1 = MainPhoneNumber;
            var number2 = "+55 49 93543-7824";

            var phoneNumber1 = new PhoneNumber(number1);
            var phoneNumber2 = new PhoneNumber(number2);

            Assert.NotEqual(phoneNumber1, phoneNumber2);
        }
        [Theory]

        [InlineData("+55 48 93543-7824", "+5548935437824")]
        [InlineData("+5548935437824", "+5548935437824")]
        [InlineData("+55 (48) 93543-7824", "+5548935437824")]
        public void ShouldCreateValidPhoneNumber(string number, string expectedValue)
        {
            var phoneNumber = new PhoneNumber(number);

            Assert.Equal(expectedValue, phoneNumber.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenNumberIsNullOrWhiteSpace(string number)
        {
            Assert.Throws<DomainValidationException>(() => new PhoneNumber(number));
        }
        [Fact]
        public void ShouldTrimNumberBeforeCreatingPhoneNumber()
        {
            var phoneNumber = new PhoneNumber($"  {MainPhoneNumber}  ");

            Assert.Equal(MainPhoneNumber, phoneNumber.Value);
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenNumberIsMoreThan50Characters()
        {
            var number = new string('1', 51);
            Assert.Throws<DomainValidationException>(() => new PhoneNumber(number));
        }
        [Fact]
        public void ShouldNotThrowLengthExceptionWhenPhoneNumberHasExactly50Characters()
        {
            var value = "+" + new string('1', 49);

            var exception = Assert.Throws<DomainValidationException>(
                () => new PhoneNumber(value)
            );

            Assert.Equal("Invalid phone number", exception.Message);
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenNumberNotStartWithPlus()
        {
            var number = "5549988887824";

            Assert.Throws<DomainValidationException>(() => new PhoneNumber(number));
        }
        [Theory]
        [InlineData("+1234567890123456")] 
        [InlineData("+12345678901234567890123456789012345678901234567890")] 
        [InlineData("1")] 
        [InlineData("+5511")]
        [InlineData("TextoInvalido")]
        [InlineData("123456789012324567890123456789012345678901234567890")]
        [InlineData("+551199999-999#")] 
        [InlineData("+551199999_9999")] 
        [InlineData("Telefone: +5511999999999")]
        public void ShouldThrowDomainValidationExceptionWhenNumberIsInvalid(string number)
        {
            Assert.Throws<DomainValidationException>(() => new PhoneNumber(number));
        }
    }
}
