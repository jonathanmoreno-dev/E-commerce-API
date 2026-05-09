using E_commerce_API.src.Domain.Enums;
using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; private set; }
        public Money TotalAmount { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public OrderStatus Status { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; } = null!;
        public Shipping Shipping { get; private set; } = null!;

        private List<Payment> _payments = new();
        public IReadOnlyCollection<Payment> Payments => _payments;

        private List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        private Order() { }
        public Order(int userId)
        {
            UserId = userId;
            Status = OrderStatus.PendingPayment;
            CreatedAt = DateTime.UtcNow;
        }
        public void SetShipping(Shipping shipping)
        {
            if (shipping is null)
                throw new ArgumentNullException("Shipping cannot be null");
            Shipping = shipping;
        }
        public void AddItem(int productId, decimal unitPrice, int quantity)
        {
            if (Status != OrderStatus.PendingPayment)
                throw new ArgumentException("Cannot modify an order in progress");
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");
            if (unitPrice <= 0)
                throw new ArgumentException("Price must be greater than zero");
            var existingItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem is null)
                _orderItems.Add(new OrderItem(productId, unitPrice, quantity));
            else
                existingItem.IncreaseQuantity(quantity);
            RecalculateTotal();
        }
        public void RefundItem(int orderItemId, int quantity)
        {
            var item = _orderItems.FirstOrDefault(x => x.OrderItemId == orderItemId);
            if (item is null)
                throw new ArgumentNullException("OrderItem cannot be null");
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            item.AddRefund(quantity);
        }
        public void CreatePayment(decimal amount, PaymentMethod paymentMethod)
        {
            if (Status != OrderStatus.PendingPayment)
                throw new InvalidOperationException("Order is not in a payable state");
            if (_payments.Any(p => p.Status == PaymentStatus.Pending))
                throw new ArgumentException("There is already a pending payment");
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero");
            
            _payments.Add(new Payment(amount, paymentMethod));
        }
        public void MarkAsPaid()
        {
            if (Status != OrderStatus.PendingPayment)
                throw new ArgumentException("Only pending orders can be paid");
            if (!_payments.Any(p => p.Status == PaymentStatus.Completed))
                throw new InvalidOperationException("No payment approved");

            Status = OrderStatus.Paid;
        }
        public void MarkAsShipped()
        {
            if (Status != OrderStatus.Paid)
                throw new ArgumentException("Only paid orders can be shipped");

            Status = OrderStatus.Shipped;
        }
        public void MarkAsDelivered()
        {
            if (Status != OrderStatus.Shipped)
                throw new ArgumentException("Only shipped orders can be delivered");

            Status = OrderStatus.Delivered;
        }
        public void Cancel()
        {
            if (Status != OrderStatus.PendingPayment && Status != OrderStatus.Paid)
                throw new ArgumentException("Only pending or paid orders can be canceled");
            Status = OrderStatus.Canceled;
        }
        public void MarkAsAbandoned()
        {
            if (Status != OrderStatus.PendingPayment)
                throw new ArgumentException("Only pending orders can be abandoned");

            Status = OrderStatus.Abandoned;
        }
        public void RecalculateTotal()
        {
            TotalAmount = new Money(_orderItems.Sum(x => x.UnitPrice.Value * x.Quantity.Value));
        }
        public void AuthorizePayment(Payment payment)
        {
            EnsurePaymentBelongsToOrder(payment);
            payment.MarkAsAuthorized();
        }
        public void CompletePayment(Payment payment)
        {
            EnsurePaymentBelongsToOrder(payment);
            payment.MarkAsCompleted();
            MarkAsPaid();
        }
        public void FailPayment(Payment payment)
        {
            EnsurePaymentBelongsToOrder(payment);
            payment.MarkAsFailed();
        }
        public void CancelPayment(Payment payment)
        {
            EnsurePaymentBelongsToOrder(payment);
            payment.MarkAsCanceled();
        }
        public void AbandonPayment(Payment payment)
        {
            EnsurePaymentBelongsToOrder(payment);
            payment.MarkAsAbandoned();
        }
        public void EnsurePaymentBelongsToOrder(Payment payment)
        {
            if (!_payments.Contains(payment))
                throw new ArgumentException("Payment does not belong to this order");
        }
    }
}
