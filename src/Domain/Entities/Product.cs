using System.Xml.Linq;
using E_commerce_API.src.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace E_commerce_API.src.Domain.Entities
{
    public class Product
    {
        public int ProductId { get; private set; }
        public ProductName Name { get; private set; } = null!;
        public ProductShortDescription ShortDescription { get; private set; } = null!;
        public ProductLongDescription LongDescription { get; private set; } = null!;
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
            ChangePrice(price);
            Stock = new Quantity(stock);
        }
        public void ChangeName(string name)
        {
            Name = new ProductName(name);
        }
        public void ChangeShortDescription(string shortDescription)
        {
            ShortDescription = new ProductShortDescription(shortDescription);
        }
        public void ChangeLongDescription(string longDescription)
        {
            LongDescription = new ProductLongDescription(longDescription);
        }
        public void ChangePrice(decimal price)
        {
            Price = new Money(price);
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
