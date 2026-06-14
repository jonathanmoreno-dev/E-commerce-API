using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class CheckoutItem
    {
        public int Id { get; private set; }
        public int CheckoutId { get; private set; }
        public Checkout Checkout { get; private set; } = null!;
        public int ProductId { get; private set; }
        public Product Product { get; private set; } = null!;
        public Money UnitPrice { get; private set; } = null!;
        public Quantity Quantity { get; private set; } = null!;

        private CheckoutItem() { }
        public CheckoutItem(int productId, Money unitPrice, Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(unitPrice);
            ArgumentNullException.ThrowIfNull(quantity);

            ProductId = productId;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
        public CheckoutItem(Product product, Money unitPrice, Quantity quantity) 
            : this(product?.Id ?? throw new ArgumentNullException(nameof(product)), unitPrice, quantity)
        {
            Product = product;
        }
        public void IncreaseQuantity(Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(quantity);

            Quantity = Quantity.Add(quantity.Value);
        }
        public void DecreaseQuantity(Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(quantity);

            Quantity = Quantity.Remove(quantity.Value);
        }
    }
}
