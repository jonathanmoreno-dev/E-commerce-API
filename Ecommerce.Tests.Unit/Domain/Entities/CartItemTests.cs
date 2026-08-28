using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.Entities
{
    public class CartItemTests
    {
        [Fact]
        public void ShouldCreateCartItemWithValidValues()
        {
            var productId = Guid.NewGuid();
            var unitPrice = new Money(99.90m);
            var quantity = new Quantity(2);

            var cartItem = new CartItem(productId, unitPrice, quantity);

            Assert.NotEqual(Guid.Empty, cartItem.Id);
            Assert.Equal(productId, cartItem.ProductId);
            Assert.Equal(unitPrice, cartItem.UnitPrice);
            Assert.Equal(quantity, cartItem.Quantity);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenUnitPriceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CartItem(Guid.NewGuid(), null!, new Quantity(2)));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenQuantityIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CartItem(Guid.NewGuid(), new Money(99.90m), null!));
        }

        [Fact]
        public void ShouldChangeQuantity()
        {
            var cartItem = new CartItem(Guid.NewGuid(), new Money(99.90m), new Quantity(2));
            var newQuantity = new Quantity(5);

            cartItem.ChangeQuantity(newQuantity);

            Assert.Equal(newQuantity, cartItem.Quantity);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenChangingQuantityToNull()
        {
            var cartItem = new CartItem(Guid.NewGuid(), new Money(99.90m), new Quantity(2));

            Assert.Throws<ArgumentNullException>(() => cartItem.ChangeQuantity(null!));
        }
    }
}