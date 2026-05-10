using System.Xml.Linq;
using E_commerce_API.src.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace E_commerce_API.src.Domain.Entities
{
    public class Product
    {
        public int ProductId { get; private set; }
        public string Name { get; private set; } = "";
        public string ShortDescription { get; private set; } = "";
        public string LongDescription { get; private set; } = "";
        public Money Price { get; private set; } = null!;
        public Quantity Stock { get; private set; } = null!;

        private List<Category> _categories = new();
        public IReadOnlyCollection<Category> Categories => _categories;
        
        private List<CartItem> _cartItems = new();
        public IReadOnlyCollection<CartItem> CartItems => _cartItems;

        private List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        private Product() { }
        public Product(string name, string shortDescription, string longDescription, decimal price, int stock)
        {
            ChangeName(name);
            ChangeShortDescription(shortDescription);
            ChangeLongDescription(longDescription);
            Price = new Money(price);
            Stock = new Quantity(stock);
        }
        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product's name cannot be empty");
            if (name.Length > 255)
                throw new ArgumentException("Product's name cannot exceed 255 characters");

            Name = name;
        }
        public void ChangeShortDescription(string shortDescription)
        {
            if (string.IsNullOrWhiteSpace(shortDescription))
                throw new ArgumentException("Product's ShortDescription cannot be empty");
            if (shortDescription.Length > 400)
                throw new ArgumentException("Product's ShortDescription cannot exceed 400 characters");

            ShortDescription = shortDescription;
        }
        public void ChangeLongDescription(string longDescription)
        {
            if (string.IsNullOrWhiteSpace(longDescription))
                throw new ArgumentException("Product's LongDescription cannot be empty");

            LongDescription = longDescription;
        }
        public void IncreaseStock(int stock)
        {
            Stock = Stock.Add(stock);
        }
        public void DecreaseStock(int stock)
        {
            Stock = Stock.Remove(stock);
        }
        public void AddCategory(Category category)
        {
            if (category is null)
                throw new ArgumentNullException(nameof(category));
            if (_categories.Any(x => x.CategoryId == category.CategoryId))
                throw new InvalidOperationException($"Category with this Id: {category.CategoryId} already in product");

            _categories.Add(category);
            category.AddProduct(this);
        }
        public void RemoveCategory(int categoryId)
        {
            var category = _categories.FirstOrDefault(x => x.CategoryId == categoryId);
            if (category is null)
                throw new ArgumentNullException(nameof(category));

            _categories.Remove(category);
            category.RemoveProduct(ProductId);
        }
    }
}
