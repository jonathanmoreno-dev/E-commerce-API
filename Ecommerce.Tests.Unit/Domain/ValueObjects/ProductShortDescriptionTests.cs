using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.ValueObjects
{
    public class ProductDescriptionTests
    {
        [Fact]
        public void ShouldBeEqualWhenDescriptionsAreTheSame()
        {
            var description = new string('b', 214);

            var productShortDescription1 = new ProductShortDescription(description);
            var productShortDescription2 = new ProductShortDescription(description);

            Assert.Equal(productShortDescription1, productShortDescription2);
        }
        [Fact]
        public void ShouldNotBeEqualWhenDescriptionsAreDifferent()
        {
            var description1 = new string('b', 214);
            var description2 = new string('c', 214);

            var productShortDescription1 = new ProductShortDescription(description1);
            var productShortDescription2 = new ProductShortDescription(description2);

            Assert.NotEqual(productShortDescription1, productShortDescription2);
        }
        [Fact]
        public void ShouldCreateValidProductShortDescription()
        {
            var description = new string('b', 214);

            var productShortDescription = new ProductShortDescription(description);

            Assert.Equal(description, productShortDescription.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenDescriptionIsNullOrWhiteSpace(string description)
        {
            Assert.Throws<DomainValidationException>(() => new ProductShortDescription(description));
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenDescriptionIsMoreThan400Characters()
        {
            var description = new string('a', 401);

            Assert.Throws<DomainValidationException>(() => new ProductShortDescription(description));
        }
        [Fact]
        public void ShouldCreateCategoryDescriptionWithExactly400Characters()
        {
            var description = new string('a', 400);

            var productShortDescription = new ProductShortDescription(description);

            Assert.Equal(description, productShortDescription.Value);
        }
    }
}
