using System.Linq;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.Entities
{
    public class OrderTests
    {
        [Fact]
        public void ShouldCreateOrderWithValidValues()
        {
            var userId = Guid.NewGuid();
            var shippingAddress = CreateShippingAddress();
            var shippingCost = new Money(30m);
            var paymentMethod = GetPaymentMethod();
            var productId = Guid.NewGuid();
            var items = new[]
            {
                (productId, new Money(99.90m), new Quantity(2))
            };
            var totalPaid = new Money(229.80m);

            var order = new Order(
                userId,
                shippingAddress,
                shippingCost,
                paymentMethod,
                items,
                totalPaid);

            Assert.NotEqual(Guid.Empty, order.Id);
            Assert.Equal(userId, order.UserId);
            Assert.Equal(shippingAddress, order.ShippingAddress);
            Assert.Equal(shippingCost, order.ShippingCost);
            Assert.Equal(paymentMethod, order.PaymentMethod);
            Assert.Equal(OrderStatus.Paid, order.Status);
            Assert.Equal(totalPaid, order.TotalPaid);
            Assert.NotNull(order.Shipping);
            Assert.Single(order.OrderItems);
        }

        [Fact]
        public void ShouldCalculateOrderTotals()
        {
            var order = CreateOrder();

            Assert.Equal(new Money(199.80m), order.SubTotal);
            Assert.Equal(new Money(229.80m), order.TotalPaid);
        }

        [Fact]
        public void ShouldCombineItemsWithTheSameProduct()
        {
            var productId = Guid.NewGuid();
            var items = new[]
            {
                (productId, new Money(99.90m), new Quantity(2)),
                (productId, new Money(99.90m), new Quantity(3))
            };

            var order = new Order(
                Guid.NewGuid(),
                CreateShippingAddress(),
                new Money(30m),
                GetPaymentMethod(),
                items,
                new Money(529.50m));

            Assert.Single(order.OrderItems);
            Assert.Equal(new Quantity(5), order.OrderItems.Single().Quantity);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenShippingAddressIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Order(
                Guid.NewGuid(),
                null!,
                new Money(30m),
                GetPaymentMethod(),
                CreateItems(),
                new Money(229.80m)));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenShippingCostIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Order(
                Guid.NewGuid(),
                CreateShippingAddress(),
                null!,
                GetPaymentMethod(),
                CreateItems(),
                new Money(229.80m)));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenItemsAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Order(
                Guid.NewGuid(),
                CreateShippingAddress(),
                new Money(30m),
                GetPaymentMethod(),
                null!,
                new Money(229.80m)));
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenItemsAreEmpty()
        {
            Assert.Throws<BusinessRuleException>(() => new Order(
                Guid.NewGuid(),
                CreateShippingAddress(),
                new Money(30m),
                GetPaymentMethod(),
                Array.Empty<(Guid productId, Money unitPrice, Quantity quantity)>(),
                new Money(30m)));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenTotalPaidIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Order(
                Guid.NewGuid(),
                CreateShippingAddress(),
                new Money(30m),
                GetPaymentMethod(),
                CreateItems(),
                null!));
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenTotalPaidDoesNotMatchExpectedTotal()
        {
            Assert.Throws<BusinessRuleException>(() => new Order(
                Guid.NewGuid(),
                CreateShippingAddress(),
                new Money(30m),
                GetPaymentMethod(),
                CreateItems(),
                new Money(100m)));
        }

        [Fact]
        public void ShouldCancelPaidOrder()
        {
            var order = CreateOrder();

            order.Cancel();

            Assert.Equal(OrderStatus.Canceled, order.Status);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenCancelingNonPaidOrder()
        {
            var order = CreateOrder();
            MarkOrderAsShipped(order);

            Assert.Throws<BusinessRuleException>(() => order.Cancel());
        }

        [Fact]
        public void ShouldThrowNotFoundExceptionWhenRefundingNonexistentItem()
        {
            var order = CreateOrder();

            Assert.Throws<NotFoundException>(() => order.RefundItem(Guid.NewGuid(), new Quantity(1)));
        }

        [Fact]
        public void ShouldSetTrackingCode()
        {
            var order = CreateOrder();
            var trackingCode = "BR123456789SC";

            order.SetTrackingCode(trackingCode);

            Assert.Equal(trackingCode, order.Shipping.TrackingCode);
        }

        [Fact]
        public void ShouldMarkOrderAsProcessing()
        {
            var order = CreateOrder();

            order.MarkAsProcessing();

            Assert.Equal(OrderStatus.Paid, order.Status);
        }

        [Fact]
        public void ShouldMarkOrderAsShipped()
        {
            var order = CreateOrder();
            order.MarkAsProcessing();
            order.SetTrackingCode("BR123456789SC");

            order.MarkAsShipped();

            Assert.Equal(OrderStatus.Shipped, order.Status);
        }

        [Fact]
        public void ShouldMarkOrderAsInTransit()
        {
            var order = CreateOrder();
            MarkOrderAsShipped(order);

            order.MarkAsInTransit();

            Assert.Equal(OrderStatus.Shipped, order.Status);
        }

        [Fact]
        public void ShouldMarkOrderAsDelivered()
        {
            var order = CreateOrder();
            MarkOrderAsShipped(order);
            order.MarkAsInTransit();

            order.MarkAsDelivered();

            Assert.Equal(OrderStatus.Delivered, order.Status);
        }

        [Fact]
        public void ShouldMarkOrderAsReturned()
        {
            var order = CreateOrder();
            MarkOrderAsShipped(order);
            order.MarkAsInTransit();
            order.MarkAsDelivered();

            order.MarkAsReturned();

            Assert.Equal(OrderStatus.Delivered, order.Status);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenMarkingAsProcessingNonPaidOrder()
        {
            var order = CreateOrder();
            MarkOrderAsShipped(order);

            Assert.Throws<BusinessRuleException>(() => order.MarkAsProcessing());
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenMarkingAsShippedNonPaidOrder()
        {
            var order = CreateOrder();
            order.MarkAsProcessing();
            order.SetTrackingCode("BR123456789SC");
            order.MarkAsShipped();

            Assert.Throws<BusinessRuleException>(() => order.MarkAsShipped());
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenMarkingAsInTransitBeforeShipped()
        {
            var order = CreateOrder();

            Assert.Throws<BusinessRuleException>(() => order.MarkAsInTransit());
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenMarkingAsDeliveredBeforeShipped()
        {
            var order = CreateOrder();

            Assert.Throws<BusinessRuleException>(() => order.MarkAsDelivered());
        }

        private static Order CreateOrder()
        {
            return new Order(
                Guid.NewGuid(),
                CreateShippingAddress(),
                new Money(30m),
                GetPaymentMethod(),
                CreateItems(),
                new Money(229.80m));
        }

        private static (Guid productId, Money unitPrice, Quantity quantity)[] CreateItems()
        {
            return new[]
            {
                (Guid.NewGuid(), new Money(99.90m), new Quantity(2))
            };
        }

        private static ShippingAddress CreateShippingAddress()
        {
            return new ShippingAddress(
                new PersonName("Maria da Silva"),
                new PhoneNumber("+5538992157062"),
                "Centro",
                "Rua Nova",
                "456",
                "Paraná",
                "Cascavel",
                "42817-000");
        }

        private static PaymentMethod GetPaymentMethod()
        {
            return Enum.GetValues<PaymentMethod>()[0];
        }

        private static void MarkOrderAsShipped(Order order)
        {
            order.MarkAsProcessing();
            order.SetTrackingCode("BR123456789SC");
            order.MarkAsShipped();
        }
    }
}
