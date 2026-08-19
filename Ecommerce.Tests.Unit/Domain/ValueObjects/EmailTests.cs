using System.Net.Mail;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.ValueObjects
{
    public class EmailTests
    {
        [Fact]
        public void ShouldBeEqualWhenEmailsAreTheSame()
        {
            var emailAddress = new string('v', 50) + "@email.com";

            var email1 = new Email(emailAddress);
            var email2 = new Email(emailAddress);

            Assert.Equal(email1, email2);
        }
        [Fact]
        public void ShouldNotBeEqualWhenEmailsAreDifferent()
        {
            var emailAddress1 = new string('v', 50) + "@email.com";
            var emailAddress2 = new string('w', 50) + "@email.com";

            var email1 = new Email(emailAddress1);
            var email2 = new Email(emailAddress2);

            Assert.NotEqual(email1, email2);
        }
        [Fact]
        public void ShouldCreateValidEmail()
        {
            var validEmail = new string('v', 50) + "@email.com";

            var email = new Email(validEmail);

            Assert.Equal(validEmail, email.Value);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenEmailIsNullOrWhiteSpace(string email)
        {
            Assert.Throws<DomainValidationException>(() => new Email(email));
        }
        [Fact]
        public void ShouldTrimEmailAddressBeforeCreatingEmail()
        {
            var emailAddress = new string('v', 50) + "@email.com";
            var email = new Email($"  {emailAddress}  ");

            Assert.Equal(emailAddress, email.Value);
        }
        [Fact]
        public void ShouldLowerUrlBeforeCreatingEmail()
        {
            var emailAddress = new string('V', 50) + "@EMAIL.com";
            var email = new Email($"  {emailAddress}  ");

            Assert.Equal(emailAddress.ToLowerInvariant(), email.Value);
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenEmailIsMoreThan255Characters()
        {
            var emailDomain = "@email.com";
            var remainingLength = 256 - emailDomain.Length;
            var email = new string('a', remainingLength) + emailDomain;

            Assert.Throws<DomainValidationException>(() => new Email(email));
        }
        [Fact]
        public void ShouldCreateEmailWithExactly255Characters()
        {
            var emailDomain = "@email.com";
            var remainingLength = 255 - emailDomain.Length;
            var emailAddress = new string('a', remainingLength) + emailDomain;

            var email = new Email(emailAddress);

            Assert.Equal(emailAddress, email.Value);
        }
        [Theory]
        [InlineData("user")]
        [InlineData("@email.com")]
        [InlineData("user@")]
        [InlineData("user@@email.com")]
        [InlineData("user email@email.com")]
        public void ShouldThrowDomainValidationExceptionWhenEmailIsInvalid(string email)
        {
            Assert.Throws<DomainValidationException>(() => new Email(email));
        }
    }
}
