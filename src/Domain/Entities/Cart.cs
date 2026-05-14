using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace E_commerce_API.src.Domain.Entities
{
    public class Cart
    {
        public int CartId { get; private set; }
        public int UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public User User { get; private set; } = null!;
        
        private readonly List<CartItem> _cartItems = new();
        public IReadOnlyCollection<CartItem> CartItems => _cartItems;

        private Cart() { }
        public Cart(int userId)
        {
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }
        public Cart(User user) : this(user.UserId)
        {
            User = user;
        }
        public void AddItem(int productId, decimal unitPrice, int quantity)
        {
            var existingItem = _cartItems.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem is null)
                _cartItems.Add(new CartItem(productId, unitPrice, quantity));
            else
                existingItem.IncreaseQuantity(quantity);
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
