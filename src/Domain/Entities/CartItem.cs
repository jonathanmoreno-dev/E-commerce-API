using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class CartItem
    {
        public int Id { get; private set; }
        public int CartId { get; private set; }
        public Cart Cart { get; private set; } = null!;
        public int ProductId { get; private set; }
        public Product Product { get; private set; } = null!;
        public Money UnitPrice { get; private set; } = null!;
        public Quantity Quantity { get; private set; } = null!;

        private CartItem() { }
        public CartItem(int productId, Money unitPrice, Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(unitPrice);
            ArgumentNullException.ThrowIfNull(quantity);

            ProductId = productId;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
        public CartItem(Product product, Money unitPrice, Quantity quantity) 
            : this(product?.Id ?? throw new ArgumentNullException(nameof(product)), unitPrice, quantity)
        {
            Product = product;
        }
        public void ChangeQuantity(Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(quantity);

            Quantity = quantity;
        }
    }
}
