using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.Entities
{
    public class ShippingTests
    {
        [Fact]
        public void ShouldCreateShippingWithValidValues()
        {
            var shippingAddress = CreateShippingAddress();
            var shippingCost = new Money(30m);

            var shipping = new Shipping(shippingAddress, shippingCost);

            Assert.NotEqual(Guid.Empty, shipping.Id);
            Assert.Equal(shippingAddress, shipping.ShippingAddress);
            Assert.Equal(shippingCost, shipping.ShippingCost);
            Assert.Equal(ShippingStatus.Pending, shipping.Status);
            Assert.Null(shipping.TrackingCode);
            Assert.Null(shipping.ShippedDate);
            Assert.Null(shipping.DeliveredDate);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenShippingAddressIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Shipping(null!, new Money(30m)));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenShippingCostIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Shipping(CreateShippingAddress(), null!));
        }

        [Fact]
        public void ShouldSetTrackingCode()
        {
            var shipping = CreateShipping();
            var trackingCode = "BR123456789SC";

            shipping.SetTrackingCode(trackingCode);

            Assert.Equal(trackingCode, shipping.TrackingCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenTrackingCodeIsNullOrWhiteSpace(string trackingCode)
        {
            var shipping = CreateShipping();

            Assert.Throws<DomainValidationException>(() => shipping.SetTrackingCode(trackingCode));
        }

        [Fact]
        public void ShouldMarkShippingAsProcessing()
        {
            var shipping = CreateShipping();

            shipping.MarkAsProcessing();

            Assert.Equal(ShippingStatus.Processing, shipping.Status);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenProcessingNonPendingShipping()
        {
            var shipping = CreateShipping();
            shipping.MarkAsProcessing();

            Assert.Throws<BusinessRuleException>(() => shipping.MarkAsProcessing());
        }

        [Fact]
        public void ShouldMarkProcessingShippingAsShipped()
        {
            var shipping = CreateShipping();
            shipping.MarkAsProcessing();

            shipping.MarkAsShipped();

            Assert.Equal(ShippingStatus.Shipped, shipping.Status);
            Assert.NotNull(shipping.ShippedDate);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenShippingIsNotProcessing()
        {
            var shipping = CreateShipping();

            Assert.Throws<BusinessRuleException>(() => shipping.MarkAsShipped());
        }

        [Fact]
        public void ShouldMarkShippedShippingAsInTransit()
        {
            var shipping = CreateShipping();
            MarkShippingAsShipped(shipping);

            shipping.MarkAsInTransit();

            Assert.Equal(ShippingStatus.InTransit, shipping.Status);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenShippingIsNotShipped()
        {
            var shipping = CreateShipping();

            Assert.Throws<BusinessRuleException>(() => shipping.MarkAsInTransit());
        }

        [Fact]
        public void ShouldMarkShippingInTransitAsDelivered()
        {
            var shipping = CreateShipping();
            MarkShippingAsShipped(shipping);
            shipping.MarkAsInTransit();

            shipping.MarkAsDelivered();

            Assert.Equal(ShippingStatus.Delivered, shipping.Status);
            Assert.NotNull(shipping.DeliveredDate);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenShippingIsNotInTransit()
        {
            var shipping = CreateShipping();

            Assert.Throws<BusinessRuleException>(() => shipping.MarkAsDelivered());
        }

        [Fact]
        public void ShouldMarkShippingAsReturned()
        {
            var shipping = CreateShipping();

            shipping.MarkAsReturned();

            Assert.Equal(ShippingStatus.Returned, shipping.Status);
        }

        private static Shipping CreateShipping()
        {
            return new Shipping(CreateShippingAddress(), new Money(30m));
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

        private static void MarkShippingAsShipped(Shipping shipping)
        {
            shipping.MarkAsProcessing();
            shipping.MarkAsShipped();
        }
    }
}
