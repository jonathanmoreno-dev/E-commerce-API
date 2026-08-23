using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.ValueObjects
{
    public class QuantityTests
    {
        [Fact]
        public void ShouldBeEqualWhenQuantityValuesAreTheSame()
        {
            var quantity1 = new Quantity(2);
            var quantity2 = new Quantity(2);

            Assert.Equal(quantity1, quantity2);
        }
        [Fact]
        public void ShouldNotBeEqualWhenQuantityValuesAreDifferent()
        {
            var quantity1 = new Quantity(2);
            var quantity2 = new Quantity(3);

            Assert.NotEqual(quantity1, quantity2);
        }
        [Fact]
        public void ShouldCreateValidQuantityWithPositiveValue()
        {
            var value = 5;
            var quantity = new Quantity(5);

            Assert.Equal(value, quantity.Value);
        }
        [Fact]
        public void ShouldCreateValidQuantityWithValueEqualToZero()
        {
            var value = 0;
            var quantity = new Quantity(value);

            Assert.Equal(value, quantity.Value);
        }
        [Theory]
        [InlineData(-5)]
        [InlineData(-3)]
        [InlineData(-1)]
        public void ShouldThrowDomainValidationExceptionWhenValueIsNegative(int value)
        {
            Assert.Throws<DomainValidationException>(() => new Quantity(value));
        }
        [Theory]
        [InlineData(-2)]
        [InlineData(-1)]
        [InlineData(-0.001)]
        [InlineData(0)]
        public void ShouldThrowDomainValidationExceptionWhenValueIsLessThanOneInAddMethod(int value)
        {
            var quantity = new Quantity(0);

            Assert.Throws<DomainValidationException>(() => quantity.Add(value));
        }
        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        [InlineData(2)]
        [InlineData(1)]
        public void ShouldAddQuantityWhenValueIsValid(int value)
        {
            var quantity = new Quantity(3);
            var expectedQuantity = value + quantity.Value;

            var quantityUpdated = quantity.Add(value);

            Assert.Equal(expectedQuantity, quantityUpdated.Value);
        }
        [Fact]
        public void ShouldNotChangeOriginalQuantityWhenAdding()
        {
            var quantity = new Quantity(5);

            var result = quantity.Add(3);

            Assert.Equal(5, quantity.Value);
            Assert.Equal(8, result.Value);
        }
        [Fact]
        public void ShouldNotChangeOriginalQuantityWhenRemoving()
        {
            var quantity = new Quantity(5);

            var result = quantity.Remove(2);

            Assert.Equal(5, quantity.Value);
            Assert.Equal(3, result.Value);
        }
        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        [InlineData(2)]
        [InlineData(1)]
        public void ShouldRemoveQuantityWhenValueIsValid(int value)
        {
            var quantity = new Quantity(4);
            var expectedQuantity = quantity.Value - value;

            var quantityUpdated = quantity.Remove(value);

            Assert.Equal(expectedQuantity, quantityUpdated.Value);
        }
        [Theory]
        [InlineData(-3)]
        [InlineData(-2)]
        [InlineData(-1)]
        [InlineData(0)]
        public void ShouldThrowDomainValidationExceptionWhenValueIsLessThanOneInRemoveMethod(int value)
        {
            var quantity = new Quantity(0);

            Assert.Throws<DomainValidationException>(() => quantity.Remove(value));
        }

        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenValueIsMoreThanCurrentlyQuantityInRemoveMethod()
        {
            var quantity = new Quantity(5);

            Assert.Throws<DomainValidationException>(() => quantity.Remove(quantity.Value - 6));
        }
        [Fact]
        public void ShouldRemoveEntireQuantity()
        {
            var quantity = new Quantity(5);

            var result = quantity.Remove(5);

            Assert.Equal(0, result.Value);
        }
    }
}
