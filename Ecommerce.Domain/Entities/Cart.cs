using System.ComponentModel;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities
{
    public class Cart
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public User User { get; private set; } = null!;
        public Money SubTotal => new Money(_cartItems.Sum(x => x.UnitPrice.Value * x.Quantity.Value));

        private readonly List<CartItem> _cartItems = new();
        public IReadOnlyCollection<CartItem> CartItems => _cartItems;

        private Cart() { }
        public Cart(int userId)
        {
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }
        public Cart(User user) : this(user?.Id ?? throw new ArgumentNullException(nameof(user)))
        {
            User = user;
        }
        public void AddItem(int productId, Money unitPrice, Quantity quantity)
        {
            var existingItem = _cartItems.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem is null)
                _cartItems.Add(new CartItem(productId, unitPrice, quantity));
            else
                existingItem.ChangeQuantity(quantity);
            UpdatedAt = DateTime.UtcNow;
        }
        public void ChangeItemQuantity(int productId, Quantity quantity)
        {
            var item = _cartItems.FirstOrDefault(x => x.ProductId == productId);
            if (item is null)
                throw new KeyNotFoundException($"CartItem with Product Id: {productId} was not found");

            if (quantity.Value == 0)
                _cartItems.Remove(item);
            else
                item.ChangeQuantity(quantity);
            UpdatedAt = DateTime.UtcNow;
        }
        public void RemoveItem(int productId)
        {
            var item = _cartItems.FirstOrDefault(x => x.ProductId == productId);
            if (item is null)
                throw new KeyNotFoundException($"CartItem with Product Id: {productId} was not found");

            _cartItems.Remove(item);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
