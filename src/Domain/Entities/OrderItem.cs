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
            ArgumentNullException.ThrowIfNull(unitPrice);
            ArgumentNullException.ThrowIfNull(quantity);

            ProductId = productId;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
        public OrderItem(Product product, Money unitPrice, Quantity quantity) : this(product?.Id ?? throw new ArgumentNullException(nameof(product)), unitPrice, quantity)
        {
            Product = product;
        }
        public void IncreaseQuantity(Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(quantity);

            Quantity = Quantity.Add(quantity.Value);
        }
        public void AddRefund(Quantity quantity)
        {
            var totalRefunded = _refunds.Sum(x => x.Quantity.Value);
            if ((totalRefunded + quantity.Value) > Quantity.Value)
                throw new InvalidOperationException("Refund quantity exceeds purchased quantity");

            _refunds.Add(new Refund(quantity));
        }
    }
}
