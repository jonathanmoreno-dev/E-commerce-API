using System.Reflection.Metadata;
using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; private set; }
        public int OrderId { get; private set; }
        public Order Order { get; private set; } = null!;
        public int ProductId { get; private set; }
        public Product Product { get; private set; } = null!;
        public Money UnitPrice { get; private set; } = null!;
        public Quantity Quantity { get; private set; } = null!;

        private readonly List<Refund> _refunds = new();
        public IReadOnlyCollection<Refund> Refunds => _refunds;

        private OrderItem() { }
        public OrderItem(int productId, Money unitPrice, Quantity quantity)
        {
            ProductId = productId;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
        public OrderItem(Product product, Money unitPrice, Quantity quantity) : this(product.Id, unitPrice, quantity)
        {
            Product = product;
        }
        public void IncreaseQuantity(Quantity quantity)
        {
            Quantity = Quantity.Add(quantity.Value);
        }
        public void DecreaseQuantity(Quantity quantity)
        {
            Quantity = Quantity.Remove(quantity.Value);
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
