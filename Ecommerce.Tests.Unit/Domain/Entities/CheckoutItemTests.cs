using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.Entities
{
    public class CheckoutItemTests
    {
        [Fact]
        public void ShouldCreateCheckoutItemWithValidValues()
        {
            var productId = Guid.NewGuid();
            var unitPrice = new Money(99.90m);
            var quantity = new Quantity(2);

            var checkoutItem = new CheckoutItem(productId, unitPrice, quantity);

            Assert.NotEqual(Guid.Empty, checkoutItem.Id);
            Assert.Equal(productId, checkoutItem.ProductId);
            Assert.Equal(unitPrice, checkoutItem.UnitPrice);
            Assert.Equal(quantity, checkoutItem.Quantity);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenUnitPriceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CheckoutItem(Guid.NewGuid(), null!, new Quantity(2)));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenQuantityIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CheckoutItem(Guid.NewGuid(), new Money(99.90m), null!));
        }

        [Fact]
        public void ShouldIncreaseQuantity()
        {
            var checkoutItem = new CheckoutItem(Guid.NewGuid(), new Money(99.90m), new Quantity(2));
            var quantityToAdd = new Quantity(3);

            checkoutItem.IncreaseQuantity(quantityToAdd);

            Assert.Equal(new Quantity(5), checkoutItem.Quantity);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenIncreasingQuantityWithNull()
        {
            var checkoutItem = new CheckoutItem(Guid.NewGuid(), new Money(99.90m), new Quantity(2));

            Assert.Throws<ArgumentNullException>(() => checkoutItem.IncreaseQuantity(null!));
        }
    }
}