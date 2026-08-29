using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.Entities
{
    public class CheckoutTests
    {
        private const PaymentMethod DefaultPaymentMethod = (PaymentMethod)1;

        [Fact]
        public void ShouldCreateCheckoutWithValidValues()
        {
            var userId = Guid.NewGuid();
            var shippingAddress = CreateShippingAddress();
            var shippingCost = new Money(30);
            var items = CreateItems();

            var checkout = new Checkout(userId, shippingAddress, shippingCost, items);

            Assert.NotEqual(Guid.Empty, checkout.Id);
            Assert.Equal(userId, checkout.UserId);
            Assert.Equal(shippingAddress, checkout.ShippingAddress);
            Assert.Equal(shippingCost, checkout.ShippingCost);
            Assert.Null(checkout.PaymentMethod);
            Assert.Empty(checkout.PaymentAttempts);
            Assert.False(checkout.HasStartedPayment);
            Assert.Null(checkout.CompletedPayment);
            Assert.True(checkout.IsActive);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenShippingAddressIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Checkout(Guid.NewGuid(), null!, new Money(30), CreateItems()));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenShippingCostIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Checkout(Guid.NewGuid(), CreateShippingAddress(), null!, CreateItems()));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenItemsAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Checkout(Guid.NewGuid(), CreateShippingAddress(), new Money(30), null!));
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenCheckoutHasNoItems()
        {
            Assert.Throws<BusinessRuleException>(() => new Checkout(
                Guid.NewGuid(),
                CreateShippingAddress(),
                new Money(30),
                new List<(Guid productId, Money unitPrice, Quantity quantity)>()));
        }

        [Fact]
        public void ShouldCalculateSubTotalAndTotal()
        {
            var items = new List<(Guid productId, Money unitPrice, Quantity quantity)>
            {
                (Guid.NewGuid(), new Money(10), new Quantity(2)),
                (Guid.NewGuid(), new Money(5), new Quantity(3))
            };

            var checkout = new Checkout(Guid.NewGuid(), CreateShippingAddress(), new Money(30), items);

            Assert.Equal(new Money(35), checkout.SubTotal);
            Assert.Equal(new Money(65), checkout.Total);
        }

        [Fact]
        public void ShouldIncreaseQuantityWhenItemsHaveTheSameProductId()
        {
            var productId = Guid.NewGuid();
            var items = new List<(Guid productId, Money unitPrice, Quantity quantity)>
            {
                (productId, new Money(10), new Quantity(2)),
                (productId, new Money(10), new Quantity(3))
            };

            var checkout = new Checkout(Guid.NewGuid(), CreateShippingAddress(), new Money(30), items);

            var checkoutItem = checkout.CheckoutItems.Single();

            Assert.Equal(productId, checkoutItem.ProductId);
            Assert.Equal(new Quantity(5), checkoutItem.Quantity);
        }

        [Fact]
        public void ShouldChangeShippingAddress()
        {
            var checkout = CreateCheckout();
            var newShippingAddress = new ShippingAddress(
                new PersonName("Maria da Silva"),
                new PhoneNumber("+5538992157062"),
                "Centro",
                "Rua Nova",
                "456",
                "Paraná",
                "Cascavel",
                "42817-000");

            checkout.ChangeShippingAddress(newShippingAddress);

            Assert.Equal(newShippingAddress, checkout.ShippingAddress);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenChangingShippingAddressToNull()
        {
            var checkout = CreateCheckout();

            Assert.Throws<ArgumentNullException>(() => checkout.ChangeShippingAddress(null!));
        }

        [Fact]
        public void ShouldChangeShippingCost()
        {
            var checkout = CreateCheckout();
            var newShippingCost = new Money(50);

            checkout.ChangeShippingCost(newShippingCost);

            Assert.Equal(newShippingCost, checkout.ShippingCost);
            Assert.Equal(checkout.SubTotal.Value + newShippingCost.Value, checkout.Total.Value);
        }
        
        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenChangingShippingCostToNull()
        {
            var checkout = CreateCheckout();

            Assert.Throws<ArgumentNullException>(() => checkout.ChangeShippingCost(null!));
        }

        [Fact]
        public void ShouldMarkExpirationAsProcessed()
        {
            var checkout = CreateCheckout();

            checkout.MarkExpirationAsProcessed();

            Assert.NotNull(checkout.ExpirationProcessedAt);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenExpirationIsAlreadyProcessed()
        {
            var checkout = CreateCheckout();

            checkout.MarkExpirationAsProcessed();

            Assert.Throws<BusinessRuleException>(() => checkout.MarkExpirationAsProcessed());
        }

        [Fact]
        public void ShouldChangePaymentMethod()
        {
            var checkout = CreateCheckout();

            checkout.ChangePaymentMethod(DefaultPaymentMethod);

            Assert.Equal(DefaultPaymentMethod, checkout.PaymentMethod);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenCreatingPaymentWithoutPaymentMethod()
        {
            var checkout = CreateCheckout();

            Assert.Throws<ArgumentNullException>(() => checkout.CreatePayment());
        }

        [Fact]
        public void ShouldCreatePendingPayment()
        {
            var checkout = CreateCheckout();
            checkout.ChangePaymentMethod(DefaultPaymentMethod);

            checkout.CreatePayment();

            var paymentAttempt = checkout.PaymentAttempts.Single();

            Assert.Equal(PaymentStatus.Pending, paymentAttempt.Status);
            Assert.Equal(checkout.Total, paymentAttempt.Amount);
            Assert.True(checkout.HasStartedPayment);
            Assert.Null(checkout.CompletedPayment);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenCreatingPaymentWithPendingPayment()
        {
            var checkout = CreateCheckoutWithPayment();

            Assert.Throws<BusinessRuleException>(() => checkout.CreatePayment());
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenCreatingPaymentWithAuthorizedPayment()
        {
            var checkout = CreateCheckoutWithPayment();
            checkout.AuthorizePayment();

            Assert.Throws<BusinessRuleException>(() => checkout.CreatePayment());
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenChangingPaymentMethodWithPendingPayment()
        {
            var checkout = CreateCheckoutWithPayment();

            Assert.Throws<BusinessRuleException>(() => checkout.ChangePaymentMethod(DefaultPaymentMethod));
        }

        [Fact]
        public void ShouldAuthorizePayment()
        {
            var checkout = CreateCheckoutWithPayment();

            checkout.AuthorizePayment();

            Assert.Equal(PaymentStatus.Authorized, checkout.PaymentAttempts.Single().Status);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenChangingPaymentMethodWithAuthorizedPayment()
        {
            var checkout = CreateCheckoutWithPayment();
            checkout.AuthorizePayment();

            Assert.Throws<BusinessRuleException>(() => checkout.ChangePaymentMethod(DefaultPaymentMethod));
        }

        [Fact]
        public void ShouldCompletePayment()
        {
            var checkout = CreateCheckoutWithPayment();
            checkout.AuthorizePayment();
            
            checkout.CompletePayment();

            var paymentAttempt = checkout.PaymentAttempts.Single();

            Assert.Equal(PaymentStatus.Completed, paymentAttempt.Status);
            Assert.Equal(paymentAttempt, checkout.CompletedPayment);
        }
        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenCompletingWithoutAuthorizePayment()
        {
            var checkout = CreateCheckoutWithPayment();

            Assert.Throws<BusinessRuleException>(() => checkout.CompletePayment());
        }
        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenCreatingPaymentWithCompletedPayment()
        {
            var checkout = CreateCheckoutWithPayment();
            checkout.AuthorizePayment();
            checkout.CompletePayment();

            Assert.Throws<BusinessRuleException>(() => checkout.CreatePayment());
        }

        [Fact]
        public void ShouldFailPayment()
        {
            var checkout = CreateCheckoutWithPayment();

            checkout.FailPayment();

            Assert.Equal(PaymentStatus.Failed, checkout.PaymentAttempts.Single().Status);
        }

        [Fact]
        public void ShouldCancelPayment()
        {
            var checkout = CreateCheckoutWithPayment();

            checkout.CancelPayment();

            Assert.Equal(PaymentStatus.Canceled, checkout.PaymentAttempts.Single().Status);
        }

        [Fact]
        public void ShouldAbandonPayment()
        {
            var checkout = CreateCheckoutWithPayment();

            checkout.AbandonPayment();

            Assert.Equal(PaymentStatus.Abandoned, checkout.PaymentAttempts.Single().Status);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenAuthorizingWithoutActivePayment()
        {
            var checkout = CreateCheckout();

            Assert.Throws<BusinessRuleException>(() => checkout.AuthorizePayment());
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenCompletingWithoutActivePayment()
        {
            var checkout = CreateCheckout();

            Assert.Throws<BusinessRuleException>(() => checkout.CompletePayment());
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenFailingWithoutActivePayment()
        {
            var checkout = CreateCheckout();

            Assert.Throws<BusinessRuleException>(() => checkout.FailPayment());
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenCancelingWithoutActivePayment()
        {
            var checkout = CreateCheckout();

            Assert.Throws<BusinessRuleException>(() => checkout.CancelPayment());
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenAbandoningWithoutActivePayment()
        {
            var checkout = CreateCheckout();

            Assert.Throws<BusinessRuleException>(() => checkout.AbandonPayment());
        }

        private static Checkout CreateCheckout()
        {
            return new Checkout(Guid.NewGuid(), CreateShippingAddress(), new Money(30), CreateItems());
        }

        private static Checkout CreateCheckoutWithPayment()
        {
            var checkout = CreateCheckout();
            checkout.ChangePaymentMethod(DefaultPaymentMethod);
            checkout.CreatePayment();

            return checkout;
        }

        private static List<(Guid productId, Money unitPrice, Quantity quantity)> CreateItems()
        {
            return new List<(Guid productId, Money unitPrice, Quantity quantity)>
            {
                (Guid.NewGuid(), new Money(10.23m), new Quantity(2)),
                (Guid.NewGuid(), new Money(100.99m), new Quantity(4)),
                (Guid.NewGuid(), new Money(329.20m), new Quantity(2))
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
    }
}