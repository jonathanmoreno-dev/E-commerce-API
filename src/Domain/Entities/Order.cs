using E_commerce_API.src.Domain.Enums;
using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class Order
    {
        public int Id { get; private set; }
        public Money SubTotal { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public OrderStatus Status { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; } = null!;
        public ShippingAddress ShippingAddress { get; private set; } = null!;
        public Money ShippingCost { get; private set; } = null!;
        public PaymentMethod PaymentMethod { get; private set; }
        public Money TotalPaid { get; private set; } = null!;
        public Shipping? Shipping { get; private set; }

        private List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        private Order() { }
        public Order(int userId, ShippingAddress shippingAddress, Money shippingCost, PaymentMethod paymentMethod, IEnumerable<(int productId, Money unitPrice, Quantity quantity)> items, Money totalPaid)
        {
            UserId = userId;
            ShippingAddress = shippingAddress;
            ShippingCost = shippingCost;
            Status = OrderStatus.Paid;
            PaymentMethod = paymentMethod;
            CreatedAt = DateTime.UtcNow;
            AddItems(items);
            CalculateTotal(totalPaid);
        }
        public Order(User user, ShippingAddress shippingAddress, Money shippingCost, PaymentMethod paymentMethod, IEnumerable<(int productId, Money unitPrice, Quantity quantity)> items, Money totalPaid) 
            : this(user.Id, shippingAddress, shippingCost, paymentMethod, items, totalPaid)
        {
            User = user;
        }

        // =========================
        //          ORDER
        // =========================
        private void CalculateTotal(Money totalPaid)
        {
            SubTotal = new Money(_orderItems.Sum(x => x.UnitPrice.Value * x.Quantity.Value));

            var expectedTotal = SubTotal.Value + ShippingCost.Value;

            if (totalPaid.Value != expectedTotal)
                throw new InvalidOperationException("Paid amount does not match expected total");

            TotalPaid = totalPaid;
        }
        private void AddItems(IEnumerable<(int productId, Money unitPrice, Quantity quantity)> items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));
            if (!items.Any())
                throw new InvalidOperationException("Order must have at least one item");

            foreach (var item in items)
            {
                var existingItem = _orderItems.FirstOrDefault(x => x.ProductId == item.productId);
                if (existingItem is null)
                    _orderItems.Add(new OrderItem(item.productId, item.unitPrice, item.quantity));
                else
                    existingItem.IncreaseQuantity(item.quantity);
            }
        }
        public void Cancel()
        {
            if (Status != OrderStatus.Paid)
                throw new InvalidOperationException("Only paid orders can be canceled");

            Status = OrderStatus.Canceled;
        }
        public void RefundItem(int orderItemId, int quantity)
        {
            var item = _orderItems.FirstOrDefault(x => x.Id == orderItemId);
            if (item is null)
                throw new KeyNotFoundException($"OrderItem with this Id {orderItemId} not found");

            item.AddRefund(quantity);
        }

        // =========================
        //          SHIPPING
        // =========================

        public void CreateShipping()
        {
            if (Shipping is not null)
                throw new InvalidOperationException("Shipping already exists");

            Shipping = new Shipping(ShippingAddress, ShippingCost);
        }
        public void MarkAsProcessing()
        {
            if (Shipping is null)
                throw new InvalidOperationException("Shipping doesn't exists");
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
    }
}
