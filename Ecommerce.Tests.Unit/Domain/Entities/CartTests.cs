using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;
using Ecommerce.Fakers;

namespace Ecommerce.Tests.Unit.Domain.Entities
{
    public class CartTests
    {
        [Fact]
        public void ShouldCreateValidCart()
        {
            var userId = Guid.NewGuid();
            var before = DateTime.UtcNow;

            var cart = new Cart(userId);

            Assert.NotEqual(Guid.Empty, cart.Id);
            Assert.Equal(userId, cart.UserId);
            Assert.True(cart.CreatedAt >= before);
            Assert.Equal(cart.CreatedAt, cart.UpdatedAt);
            Assert.Empty(cart.CartItems);
        }
        [Theory]
        [InlineData(100.20, 2)]
        [InlineData(1.20, 1)]
        [InlineData(0.01, 10)]
        public void ShouldAddNewItemWithValidValues(decimal money, int value)
        {
            var cart = new Cart(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var unitPrice = new Money(money);
            var quantity = new Quantity(value);

            cart.AddItem(productId, unitPrice, quantity);
            var item = cart.CartItems.First();

            Assert.Equal(quantity.Value, item.Quantity.Value);
        }
        [Fact]
        public void ShouldIncreaseQuantityWhenItemAlreadyExists()
        {
            var cart = new Cart(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var unitPrice = new Money(5.30m);
            var quantity = new Quantity(2);
            cart.AddItem(productId, unitPrice, quantity);
            var finalQuantity = 4;

            cart.AddItem(productId, unitPrice, new Quantity(finalQuantity));
            var item = cart.CartItems.First();

            Assert.Equal(finalQuantity, item.Quantity.Value);
        }
        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenQuantityIsEqualToZeroInAddItemMethod()
        {
            var cart = new Cart(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var unitPrice = new Money(100);
            var quantity = new Quantity(0);

            Assert.Throws<BusinessRuleException>(() => cart.AddItem(productId, unitPrice, quantity));
        }

        [Fact]
        public void ShouldChangeItemQuantity()
        {
            var cart = new Cart(Guid.NewGuid());
            var productId = Guid.NewGuid();
            cart.AddItem(productId, new Money(100), new Quantity(2));

            var quantity = new Quantity(5);
            cart.ChangeItemQuantity(productId, quantity);

            var item = cart.CartItems.First();

            Assert.Equal(quantity.Value, item.Quantity.Value);
        }

        [Fact]
        public void ShouldRemoveItemWhenChangingQuantityToZero()
        {
            var cart = new Cart(Guid.NewGuid());
            var productId = Guid.NewGuid();
            cart.AddItem(productId, new Money(100), new Quantity(2));

            cart.ChangeItemQuantity(productId, new Quantity(0));

            Assert.Empty(cart.CartItems);
        }

        [Fact]
        public void ShouldThrowNotFoundExceptionWhenChangingQuantityOfNonexistent_Item()
        {
            var cart = new Cart(Guid.NewGuid());
            var productId = Guid.NewGuid();

            Assert.Throws<NotFoundException>(() => cart.ChangeItemQuantity(productId, new Quantity(2)));
        }

        [Fact]
        public void ShouldRemoveItem()
        {
            var cart = new Cart(Guid.NewGuid());
            var productId = Guid.NewGuid();
            cart.AddItem(productId, new Money(100), new Quantity(2));

            cart.RemoveItem(productId);

            Assert.Empty(cart.CartItems);
        }
        [Fact]
        public void ShouldThrowNotFoundExceptionWhenRemovingNonexistentItem()
        {
            var cart = new Cart(Guid.NewGuid());
            var productId = Guid.NewGuid();

            Assert.Throws<NotFoundException>(() => cart.RemoveItem(productId));
        }

        [Fact]
        public void ShouldClearAllItems()
        {
            var cart = new Cart(Guid.NewGuid());
            cart.AddItem(Guid.NewGuid(), new Money(100), new Quantity(2));
            cart.AddItem(Guid.NewGuid(), new Money(50), new Quantity(3));

            cart.ClearItems();

            Assert.Empty(cart.CartItems);
        }

        [Fact]
        public void ShouldCalculateSubTotal()
        {
            var cart = new Cart(Guid.NewGuid());
            cart.AddItem(Guid.NewGuid(), new Money(100), new Quantity(2));
            cart.AddItem(Guid.NewGuid(), new Money(50), new Quantity(3));

            var subtotal = cart.SubTotal;
            var subTotalExpected = cart.CartItems.Sum(x => x.UnitPrice.Value * x.Quantity.Value);

            Assert.Equal(subTotalExpected, subtotal.Value);
        }
    }
}
