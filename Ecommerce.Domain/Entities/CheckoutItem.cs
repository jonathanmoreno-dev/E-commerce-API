using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities
{
    public class CheckoutItem
    {
        public Guid Id { get; private set; }
        public Guid CheckoutId { get; private set; }
        public Checkout Checkout { get; private set; } = null!;
        public Guid ProductId { get; private set; }
        public Product Product { get; private set; } = null!;
        public Money UnitPrice { get; private set; } = null!;
        public Quantity Quantity { get; private set; } = null!;

        private CheckoutItem() { }
        public CheckoutItem(Guid productId, Money unitPrice, Quantity quantity)
        {
            Id = Guid.NewGuid();

            ArgumentNullException.ThrowIfNull(unitPrice);
            ArgumentNullException.ThrowIfNull(quantity);

            ProductId = productId;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
        public void IncreaseQuantity(Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(quantity);

            Quantity = Quantity.Add(quantity.Value);
        }
    }
}
