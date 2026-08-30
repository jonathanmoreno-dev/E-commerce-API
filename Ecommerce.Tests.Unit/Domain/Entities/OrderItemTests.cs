using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.Entities
{
    public class OrderItemTests
    {
        [Fact]
        public void ShouldCreateOrderItemWithValidValues()
        {
            var productId = Guid.NewGuid();
            var unitPrice = new Money(99.90m);
            var quantity = new Quantity(2);

            var orderItem = new OrderItem(productId, unitPrice, quantity);

            Assert.NotEqual(Guid.Empty, orderItem.Id);
            Assert.Equal(productId, orderItem.ProductId);
            Assert.Equal(unitPrice, orderItem.UnitPrice);
            Assert.Equal(quantity, orderItem.Quantity);
            Assert.Empty(orderItem.Refunds);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenUnitPriceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new OrderItem(Guid.NewGuid(), null!, new Quantity(2)));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenQuantityIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new OrderItem(Guid.NewGuid(), new Money(99.90m), null!));
        }

        [Fact]
        public void ShouldIncreaseQuantity()
        {
            var orderItem = new OrderItem(Guid.NewGuid(), new Money(99.90m), new Quantity(2));
            var quantityToAdd = new Quantity(3);

            orderItem.IncreaseQuantity(quantityToAdd);

            Assert.Equal(new Quantity(5), orderItem.Quantity);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenIncreasingQuantityWithNull()
        {
            var orderItem = new OrderItem(Guid.NewGuid(), new Money(99.90m), new Quantity(2));

            Assert.Throws<ArgumentNullException>(() => orderItem.IncreaseQuantity(null!));
        }

        [Fact]
        public void ShouldAddRefund()
        {
            var orderItem = new OrderItem(Guid.NewGuid(), new Money(99.90m), new Quantity(5));
            var refundQuantity = new Quantity(2);

            orderItem.AddRefund(refundQuantity);

            var refund = Assert.Single(orderItem.Refunds);

            Assert.Equal(refundQuantity, refund.Quantity);
        }

        [Fact]
        public void ShouldAllowMultipleRefundsWithinPurchasedQuantity()
        {
            var orderItem = new OrderItem(Guid.NewGuid(), new Money(99.90m), new Quantity(5));

            orderItem.AddRefund(new Quantity(2));
            orderItem.AddRefund(new Quantity(3));

            Assert.Equal(2, orderItem.Refunds.Count);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenRefundQuantityExceedsPurchasedQuantity()
        {
            var orderItem = new OrderItem(Guid.NewGuid(), new Money(99.90m), new Quantity(5));

            Assert.Throws<BusinessRuleException>(() => orderItem.AddRefund(new Quantity(6)));
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenTotalRefundQuantityExceedsPurchasedQuantity()
        {
            var orderItem = new OrderItem(Guid.NewGuid(), new Money(99.90m), new Quantity(5));
            orderItem.AddRefund(new Quantity(3));

            Assert.Throws<BusinessRuleException>(() => orderItem.AddRefund(new Quantity(3)));
        }
    }
}
