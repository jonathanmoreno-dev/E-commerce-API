using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.ValueObjects
{
    public class ProductLongDescriptionTests
    {
        [Fact]
        public void ShouldBeEqualWhenDescriptionsAreTheSame()
        {
            var description = new string('b', 414);

            var productLongDescription1 = new ProductLongDescription(description);
            var productLongDescription2 = new ProductLongDescription(description);

            Assert.Equal(productLongDescription1, productLongDescription2);
        }
        [Fact]
        public void ShouldNotBeEqualWhenDescriptionsAreDifferent()
        {
            var description1 = new string('b', 414);
            var description2 = new string('c', 414);

            var productLongDescription1 = new ProductLongDescription(description1);
            var productLongDescription2 = new ProductLongDescription(description2);

            Assert.NotEqual(productLongDescription1, productLongDescription2);
        }
        [Fact]
        public void ShouldCreateValidProductLongDescription()
        {
            var description = new string('b', 414);

            var productLongDescription = new ProductLongDescription(description);

            Assert.Equal(description, productLongDescription.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenDescriptionIsNullOrWhiteSpace(string description)
        {
            Assert.Throws<DomainValidationException>(() => new ProductLongDescription(description));
        }
    }
}
