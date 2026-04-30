using System.Reflection.Metadata;

namespace E_commerce_API.src.Domain.Entities
{
    public class OrderItem
    {
        public int OrderItemId { get; private set; }
        public int OrderId { get; private set; }
        public Order Order { get; private set; } = null!;
        public int ProductId { get; private set; }
        public Product Product { get; private set; } = null!;
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }

        private readonly List<Refund> _refunds = new();
        public IReadOnlyCollection<Refund> Refunds => _refunds;

        private OrderItem() { }
        public OrderItem(int productId, decimal unitPrice, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            ProductId = productId;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
        public void IncreaseQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            Quantity += quantity;
        }
        public void DecreaseQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");
            if ((Quantity - quantity) < 0)
                throw new ArgumentException("Quantity cannot be negative");

            Quantity -= quantity;
        }
        public void AddRefund(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var totalRefunded = _refunds.Sum(x => x.Quantity.Value);
            if ((totalRefunded + quantity) > Quantity)
                throw new ArgumentException("Refund exceeds purchased quantity");

            _refunds.Add(new Refund(quantity));
        }
    }
}
