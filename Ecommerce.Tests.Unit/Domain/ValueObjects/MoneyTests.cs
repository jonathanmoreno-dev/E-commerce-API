using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.ValueObjects
{
    public class MoneyTests
    {
        [Fact]
        public void ShouldBeEqualWhenMoneyValuesAreTheSame()
        {
            var money1 = new Money(5.34m);
            var money2 = new Money(5.34m);

            Assert.Equal(money1, money2);
        }
        [Fact]
        public void ShouldNotBeEqualWhenMoneyValuesAreDifferent()
        {
            var money1 = new Money(5.34m);
            var money2 = new Money(7.41m);

            Assert.NotEqual(money1, money2);
        }
        [Theory]
        [InlineData(0.01)]
        [InlineData(10.99)]
        [InlineData(999999.99)]
        public void ShouldCreateValidMoneyWithPositiveValue(decimal value)
        {
            var money = new Money(value);

            Assert.Equal(value, money.Value);
        }
        [Fact]
        public void ShouldCreateValidMoneyWithValueEqualToZero()
        {
            var value = 0m;
            var money = new Money(value);

            Assert.Equal(value, money.Value);
        }
        [Theory]
        [InlineData(-0.01)]
        [InlineData(-5.99)]
        [InlineData(-100)]
        public void ShouldThrowDomainValidationExceptionWhenValueIsNegative(decimal value)
        {
            Assert.Throws<DomainValidationException>(() => new Money(value));
        }
    }
}
