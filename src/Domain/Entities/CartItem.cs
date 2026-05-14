using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class CartItem
    {
        public int CartItemId { get; private set; }
        public int CartId { get; private set; }
        public Cart Cart { get; private set; } = null!;
        public int ProductId { get; private set; }
        public Product Product { get; private set; } = null!;
        public Money UnitPrice { get; private set; } = null!;
        public Quantity Quantity { get; private set; } = null!;

        private CartItem() { }
        public CartItem(int productId, decimal unitPrice, int quantity)
        {
            ProductId = productId;
            UnitPrice = new Money(unitPrice);
            Quantity = new Quantity(quantity);
        }
        public CartItem(Product product, decimal unitPrice, int quantity) : this(product.ProductId, unitPrice, quantity)
        {
            Product = product;
        }
        public void IncreaseQuantity(int quantity)
        {
            Quantity = Quantity.Add(quantity);
        }
        public void DecreaseQuantity(int quantity)
        {
            Quantity = Quantity.Remove(quantity);
        }
    }
}
