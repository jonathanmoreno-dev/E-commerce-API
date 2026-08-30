using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.Entities
{
    public class PaymentAttemptTests
    {
        [Fact]
        public void ShouldCreatePaymentAttemptWithValidValues()
        {
            var amount = new Money(229.80m);
            var method = GetPaymentMethod();

            var paymentAttempt = new PaymentAttempt(amount, method);

            Assert.NotEqual(Guid.Empty, paymentAttempt.Id);
            Assert.Equal(amount, paymentAttempt.Amount);
            Assert.Equal(method, paymentAttempt.Method);
            Assert.Equal(PaymentStatus.Pending, paymentAttempt.Status);
            Assert.NotEqual(DateTime.MinValue, paymentAttempt.PaymentDate);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenAmountIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new PaymentAttempt(null!, GetPaymentMethod()));
        }

        [Fact]
        public void ShouldMarkPendingPaymentAsAuthorized()
        {
            var paymentAttempt = CreatePaymentAttempt();

            paymentAttempt.MarkAsAuthorized();

            Assert.Equal(PaymentStatus.Authorized, paymentAttempt.Status);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenAuthorizingNonPendingPayment()
        {
            var paymentAttempt = CreatePaymentAttempt();
            paymentAttempt.MarkAsAuthorized();

            Assert.Throws<BusinessRuleException>(() => paymentAttempt.MarkAsAuthorized());
        }

        [Fact]
        public void ShouldMarkAuthorizedPaymentAsCompleted()
        {
            var paymentAttempt = CreatePaymentAttempt();
            paymentAttempt.MarkAsAuthorized();

            paymentAttempt.MarkAsCompleted();

            Assert.Equal(PaymentStatus.Completed, paymentAttempt.Status);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenCompletingNonAuthorizedPayment()
        {
            var paymentAttempt = CreatePaymentAttempt();

            Assert.Throws<BusinessRuleException>(() => paymentAttempt.MarkAsCompleted());
        }

        [Fact]
        public void ShouldMarkPendingPaymentAsFailed()
        {
            var paymentAttempt = CreatePaymentAttempt();

            paymentAttempt.MarkAsFailed();

            Assert.Equal(PaymentStatus.Failed, paymentAttempt.Status);
        }

        [Fact]
        public void ShouldMarkAuthorizedPaymentAsFailed()
        {
            var paymentAttempt = CreatePaymentAttempt();
            paymentAttempt.MarkAsAuthorized();

            paymentAttempt.MarkAsFailed();

            Assert.Equal(PaymentStatus.Failed, paymentAttempt.Status);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenFailingCompletedPayment()
        {
            var paymentAttempt = CreatePaymentAttempt();
            paymentAttempt.MarkAsAuthorized();
            paymentAttempt.MarkAsCompleted();

            Assert.Throws<BusinessRuleException>(() => paymentAttempt.MarkAsFailed());
        }

        [Fact]
        public void ShouldMarkPendingPaymentAsCanceled()
        {
            var paymentAttempt = CreatePaymentAttempt();

            paymentAttempt.MarkAsCanceled();

            Assert.Equal(PaymentStatus.Canceled, paymentAttempt.Status);
        }

        [Fact]
        public void ShouldMarkAuthorizedPaymentAsCanceled()
        {
            var paymentAttempt = CreatePaymentAttempt();
            paymentAttempt.MarkAsAuthorized();

            paymentAttempt.MarkAsCanceled();

            Assert.Equal(PaymentStatus.Canceled, paymentAttempt.Status);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenCancelingCompletedPayment()
        {
            var paymentAttempt = CreatePaymentAttempt();
            paymentAttempt.MarkAsAuthorized();
            paymentAttempt.MarkAsCompleted();

            Assert.Throws<BusinessRuleException>(() => paymentAttempt.MarkAsCanceled());
        }

        [Fact]
        public void ShouldMarkPendingPaymentAsAbandoned()
        {
            var paymentAttempt = CreatePaymentAttempt();

            paymentAttempt.MarkAsAbandoned();

            Assert.Equal(PaymentStatus.Abandoned, paymentAttempt.Status);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenAbandoningNonPendingPayment()
        {
            var paymentAttempt = CreatePaymentAttempt();
            paymentAttempt.MarkAsAuthorized();

            Assert.Throws<BusinessRuleException>(() => paymentAttempt.MarkAsAbandoned());
        }

        private static PaymentAttempt CreatePaymentAttempt()
        {
            return new PaymentAttempt(new Money(229.80m), GetPaymentMethod());
        }

        private static PaymentMethod GetPaymentMethod()
        {
            return Enum.GetValues<PaymentMethod>()[0];
        }
    }
}
