using E_commerce_API.src.Domain.Enums;
using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; private set; }
        public Money TotalAmount => new Money(_orderItems.Sum(x => x.UnitPrice.Value * x.Quantity.Value));
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
        public Order(User user) : this(user.UserId)
        {
            User = user;
        }

        // =========================
        //          ORDER
        // =========================

        public void AddItem(int productId, decimal unitPrice, int quantity)
        {
            if (Status != OrderStatus.PendingPayment)
                throw new InvalidOperationException("Cannot modify an order in progress");

            var existingItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem is null)
                _orderItems.Add(new OrderItem(productId, unitPrice, quantity));
            else
                existingItem.IncreaseQuantity(quantity);
        }
        public void MarkAsPaid()
        {
            if (Status != OrderStatus.PendingPayment)
                throw new InvalidOperationException("Only pending orders can be paid");
            if (!_payments.Any(p => p.Status == PaymentStatus.Completed))
                throw new InvalidOperationException("No payment approved");

            Status = OrderStatus.Paid;
        }
        public void Cancel()
        {
            if (Status != OrderStatus.PendingPayment && Status != OrderStatus.Paid)
                throw new InvalidOperationException("Only pending or paid orders can be canceled");
            Status = OrderStatus.Canceled;
        }
        public void MarkAsAbandoned()
        {
            if (Status != OrderStatus.PendingPayment)
                throw new InvalidOperationException("Only pending orders can be abandoned");

            Status = OrderStatus.Abandoned;
        }
        public void RefundItem(int orderItemId, int quantity)
        {
            var item = _orderItems.FirstOrDefault(x => x.OrderItemId == orderItemId);
            if (item is null)
                throw new KeyNotFoundException($"OrderItem with this Id {orderItemId} not found");

            item.AddRefund(quantity);
        }

        // =========================
        //          SHIPPING
        // =========================

        public void CreateShipping(string recipientName, string phoneNumber, string neighborhood, string street, string number, string state, string city, string zipCode, decimal shippingCost)
        {
            if (Shipping is not null)
                throw new InvalidOperationException("Shipping already exists");

            Shipping = new Shipping(recipientName, phoneNumber, neighborhood, street, number, state, city, zipCode, shippingCost);
        }
        public void MarkAsProcessing()
        {
            if (Shipping is null)
                throw new InvalidOperationException("Shipping already exists");
            if (Status != OrderStatus.Paid)
                throw new InvalidOperationException("Only paid orders can be ProcessingShipping");

            Shipping.MarkAsProcessing();
        }
        public void MarkAsShipped()
        {
            if (Shipping is null)
                throw new InvalidOperationException("Shipping doesn't exists");

            if (Status != OrderStatus.Paid)
                throw new InvalidOperationException("Only paid orders can be shipped");

            Shipping.MarkAsShipped();
            Status = OrderStatus.Shipped;
        }
        public void MarkAsInTransit()
        {
            if (Shipping is null)
                throw new InvalidOperationException("Shipping doesn't exists");

            if (Status != OrderStatus.Shipped)
                throw new InvalidOperationException("Only shipped orders can be in transit");

            Shipping.MarkAsInTransit();
        }
        public void MarkAsDelivered()
        {
            if (Shipping is null)
                throw new InvalidOperationException("Shipping doesn't exists");

            if (Status != OrderStatus.Shipped)
                throw new InvalidOperationException("Only shipped orders can be delivered");

            Shipping.MarkAsDelivered();
            Status = OrderStatus.Delivered;
        }
        public void MarkAsReturned()
        {
            if (Shipping is null)
                throw new InvalidOperationException("Shipping doesn't exists");

            Shipping.MarkAsReturned();
        }

        // =========================
        //          PAYMENT
        // =========================

        public void CreatePayment(decimal amount, PaymentMethod paymentMethod)
        {
            if (Status != OrderStatus.PendingPayment)
                throw new InvalidOperationException("Order is not in a payable state");
            if (_payments.Any(p => p.Status == PaymentStatus.Pending))
                throw new InvalidOperationException("There is already a pending payment");

            _payments.Add(new Payment(amount, paymentMethod));
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

        // =========================
        //          HELPER
        // =========================

        private void EnsurePaymentBelongsToOrder(Payment payment)
        {
            if (!_payments.Contains(payment))
                throw new InvalidOperationException("Payment does not belong to this order");
        }
    }
}
