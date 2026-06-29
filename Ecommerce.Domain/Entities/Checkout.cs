using System.Collections.Generic;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities
{
    public class Checkout
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; } = null!;
        public ShippingAddress ShippingAddress { get; private set; } = null!;
        public Money ShippingCost { get; private set; } = null!;
        public Money SubTotal => new Money(_checkoutItems.Sum(x => x.UnitPrice.Value * x.Quantity.Value));
        public Money Total => new Money(SubTotal.Value + ShippingCost.Value);
        public PaymentMethod PaymentMethod { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool IsActive => ExpiresAt > DateTime.UtcNow;
        public bool HasStartedPayment => _paymentAttempts.Any();
        private readonly List<PaymentAttempt> _paymentAttempts = new();
        public IReadOnlyCollection<PaymentAttempt> PaymentAttempts => _paymentAttempts;

        private readonly List<CheckoutItem> _checkoutItems = new();
        public IReadOnlyCollection<CheckoutItem> CheckoutItems => _checkoutItems;
        private Checkout() { }
        public Checkout(int userId, ShippingAddress shippingAddress, Money shippingCost, PaymentMethod paymentMethod, IEnumerable<(int productId, Money unitPrice, Quantity quantity)> items)
        {
            UserId = userId;
            ChangePaymentMethod(paymentMethod);
            ChangeShippingAddress(shippingAddress);
            ChangeShippingCost(shippingCost);
            AddItems(items);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
            ExpiresAt = CreatedAt.AddHours(3);
        }
        private void AddItems(IEnumerable<(int productId, Money unitPrice, Quantity quantity)> items)
        {
            ArgumentNullException.ThrowIfNull(items);
            if (!items.Any())
                throw new InvalidOperationException("Checkout must have at least one item");

            foreach (var item in items)
            {
                var existingItem = _checkoutItems.FirstOrDefault(x => x.ProductId == item.productId);
                if (existingItem is null)
                    _checkoutItems.Add(new CheckoutItem(item.productId, item.unitPrice, item.quantity));
                else
                    existingItem.IncreaseQuantity(item.quantity);
            }
            UpdatedAt = DateTime.UtcNow;
        }
        // =========================
        //          ADDRESS
        // =========================

        public void ChangeShippingAddress(ShippingAddress shippingAddress)
        {
            ArgumentNullException.ThrowIfNull(shippingAddress);
            ShippingAddress = shippingAddress;
            UpdatedAt = DateTime.UtcNow;
        }
        public void ChangeShippingCost(Money shippingCost)
        {
            ArgumentNullException.ThrowIfNull(shippingCost);
            ShippingCost = shippingCost;
            UpdatedAt = DateTime.UtcNow;
        }

        // =========================
        //          PAYMENT
        // =========================

        public void ChangePaymentMethod(PaymentMethod paymentMethod)
        {
            if(_paymentAttempts.Any(x => x.Status == PaymentStatus.Pending || x.Status == PaymentStatus.Authorized))
                throw new InvalidOperationException("Cannot change payment method while payment is in progress.");

            PaymentMethod = paymentMethod;
            UpdatedAt = DateTime.UtcNow;
        }
        public void CreatePayment()
        {
            if (_paymentAttempts.Any(p => p.Status == PaymentStatus.Pending))
                throw new InvalidOperationException("There is already a pending payment");
            if(_paymentAttempts.Any(p => p.Status == PaymentStatus.Authorized) || _paymentAttempts.Any(p => p.Status == PaymentStatus.Completed))
                throw new InvalidOperationException("There is already a authorized payment");

            _paymentAttempts.Add(new PaymentAttempt(Total, PaymentMethod));
            UpdatedAt = DateTime.UtcNow;
        }
        public void AuthorizePayment()
        {
            GetCurrentPayment().MarkAsAuthorized();
        }
        public void CompletePayment()
        {
            GetCurrentPayment().MarkAsCompleted();
        }
        public void FailPayment()
        {
            GetCurrentPayment().MarkAsFailed();
        }
        public void CancelPayment()
        {
            GetCurrentPayment().MarkAsCanceled();
        }
        public void AbandonPayment()
        {
            GetCurrentPayment().MarkAsAbandoned();
        }

        // =========================
        //          HELPER
        // =========================

        private PaymentAttempt GetCurrentPayment()
        {
            return _paymentAttempts.LastOrDefault(p =>
                p.Status == PaymentStatus.Pending ||
                p.Status == PaymentStatus.Authorized)
                ?? throw new InvalidOperationException("There is no active payment");
        }
    }
}
