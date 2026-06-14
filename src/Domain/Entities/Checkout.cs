using E_commerce_API.src.Domain.Enums;
using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
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
        private List<PaymentAttempt> _paymentAttempts = new();
        public IReadOnlyCollection<PaymentAttempt> PaymentAttempts => _paymentAttempts;

        private List<CheckoutItem> _checkoutItems = new();
        public IReadOnlyCollection<CheckoutItem> CheckoutItems => _checkoutItems;
        private Checkout() { }
        public Checkout(int userId, ShippingAddress shippingAddress, Money shippingCost, PaymentMethod paymentMethod, IEnumerable<(int productId, Money unitPrice, Quantity quantity)> items)
        {
            PaymentMethod = paymentMethod;
            ShippingAddress = shippingAddress;
            ShippingCost = shippingCost;
            AddItems(items);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
            ExpiresAt = CreatedAt.AddHours(3);
        }
        public Checkout(User user, ShippingAddress shippingAddress, Money shippingCost, PaymentMethod paymentMethod, IEnumerable<(int productId, Money unitPrice, Quantity quantity)> items)
    : this(user.Id, shippingAddress, shippingCost, paymentMethod, items)
        {
            User = user;
        }
        private void AddItems(IEnumerable<(int productId, Money unitPrice, Quantity quantity)> items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));
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
            ShippingAddress = shippingAddress;
            UpdatedAt = DateTime.UtcNow;
        }

        // =========================
        //          PAYMENT
        // =========================

        public void CreatePayment(Money amount)
        {
            if (_paymentAttempts.Any(p => p.Status == PaymentStatus.Pending))
                throw new InvalidOperationException("There is already a pending payment");
            if(_paymentAttempts.Any(p => p.Status == PaymentStatus.Authorized) || _paymentAttempts.Any(p => p.Status == PaymentStatus.Completed))
                throw new InvalidOperationException("There is already a authorized payment");

            _paymentAttempts.Add(new PaymentAttempt(amount, PaymentMethod));
            UpdatedAt = DateTime.UtcNow;
        }

        public void AuthorizePayment(PaymentAttempt paymentAttempt)
        {
            EnsurePaymentBelongsToChechout(paymentAttempt);
            paymentAttempt.MarkAsAuthorized();
        }
        public void CompletePayment(PaymentAttempt paymentAttempt)
        {
            EnsurePaymentBelongsToChechout(paymentAttempt);
            paymentAttempt.MarkAsCompleted();
            // Maybe I should create an order entity here, but I'm not sure about that
        }
        public void FailPayment(PaymentAttempt paymentAttempt)
        {
            EnsurePaymentBelongsToChechout(paymentAttempt);
            paymentAttempt.MarkAsFailed();
        }
        public void CancelPayment(PaymentAttempt paymentAttempt)
        {
            EnsurePaymentBelongsToChechout(paymentAttempt);
            paymentAttempt.MarkAsCanceled();
        }
        public void AbandonPayment(PaymentAttempt paymentAttempt)
        {
            EnsurePaymentBelongsToChechout(paymentAttempt);
            paymentAttempt.MarkAsAbandoned();
        }

        // =========================
        //          HELPER
        // =========================

        private void EnsurePaymentBelongsToChechout(PaymentAttempt paymentAttempt)
        {
            if (!_paymentAttempts.Contains(paymentAttempt))
                throw new InvalidOperationException("Payment does not belong to this checkout");
        }
    }
}
