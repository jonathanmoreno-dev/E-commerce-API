using System.Reflection.Metadata;
using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class OrderItem
    {
        public int OrderItemId { get; private set; }
        public int OrderId { get; private set; }
        public Order Order { get; private set; } = null!;
        public int ProductId { get; private set; }
        public Product Product { get; private set; } = null!;
        public Money UnitPrice { get; private set; } = null!;
        public Quantity Quantity { get; private set; } = null!;

        private readonly List<Refund> _refunds = new();
        public IReadOnlyCollection<Refund> Refunds => _refunds;

        private OrderItem() { }
        public OrderItem(int productId, decimal unitPrice, int quantity)
        {
            ProductId = productId;
            UnitPrice = new Money(unitPrice);
            Quantity = new Quantity(quantity);
        }
        public void IncreaseQuantity(int quantity)
        {
            Quantity = Quantity.Add(quantity);
        }
        public void DecreaseQuantity(int quantity)
        {
            Quantity = Quantity.Remove(quantity);
        }
        public void AddRefund(int quantity)
        {
            var totalRefunded = _refunds.Sum(x => x.Quantity.Value);
            if ((totalRefunded + quantity) > Quantity.Value)
                throw new InvalidOperationException("Refund quantity exceeds purchased quantity");

            _refunds.Add(new Refund(quantity));
        }
    }
}
