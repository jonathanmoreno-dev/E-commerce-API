using System;
using System.Xml.Linq;
using E_commerce_API.src.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace E_commerce_API.src.Domain.Entities
{
    public class Product
    {
        public int Id { get; private set; }
        public ProductName Name { get; private set; } = null!;
        public ProductShortDescription ShortDescription { get; private set; } = null!;
        public ProductLongDescription LongDescription { get; private set; } = null!;
        public Money Price { get; private set; } = null!;
        public Quantity Stock { get; private set; } = null!;
        public List<ProductImage> _productImages = new();
        public IReadOnlyCollection<ProductImage> ProductImages => _productImages;
        private List<Category> _categories = new();
        public IReadOnlyCollection<Category> Categories => _categories;
        
        private List<CartItem> _cartItems = new();
        public IReadOnlyCollection<CartItem> CartItems => _cartItems;
        private List<CheckoutItem> _checkoutItems = new();
        public IReadOnlyCollection<CheckoutItem> CheckoutItems => _checkoutItems;
        private List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        private Product() { }
        public Product(ProductName name, ProductShortDescription shortDescription, ProductLongDescription longDescription, Money price, Quantity stock)
        {
            ChangeName(name);
            ChangeShortDescription(shortDescription);
            ChangeLongDescription(longDescription);
            ChangePrice(price);
            Stock = stock;
        }
        public void ChangeName(ProductName name)
        {
            Name = name;
        }
        public void ChangeShortDescription(ProductShortDescription shortDescription)
        {
            ShortDescription = shortDescription;
        }
        public void ChangeLongDescription(ProductLongDescription longDescription)
        {
            LongDescription = longDescription;
        }
        public void ChangePrice(Money price)
        {
            Price = price;
        }
        public void AddProductImage(string url, int order)
        {
            if (_productImages.Any(x => x.Url == url))
                throw new InvalidOperationException($"ProductImage with Url: {url} already in product image");
            if(_productImages.Any(x => x.Order == order))
                throw new InvalidOperationException($"This order: {order} already in use");

            _productImages.Add(new ProductImage(url, order));
            OrganizeProductImageOrder();
        }
        public void RemoveProductImage(string url)
        {
            var productImage = _productImages.FirstOrDefault(x => x.Url == url);
            if (productImage is null)
                throw new KeyNotFoundException($"ProductImage with Url: {url} was not found");

            _productImages.Remove(productImage);
            OrganizeProductImageOrder();
        }
        private void OrganizeProductImageOrder() 
        {
            var ordered = _productImages.OrderBy(x => x.Order).ToList();

            for (int i = 0; i < ordered.Count; i++)
            {
                ordered[i] = new ProductImage(ordered[i].Url, i + 1);
            }
            _productImages.Clear();
            _productImages.AddRange(ordered);
        }
        public void ChangeOrderProcutImage(string url, int order)
        {
            var productImage = _productImages.FirstOrDefault(x => x.Url == url);
            if(productImage is null)
                throw new KeyNotFoundException($"ProductImage with Url: {url} was not found");
            if (_productImages.Any(x => x.Order == order))
                throw new InvalidOperationException($"This order: {order} already in use");
            if (order > _productImages.Count)
                throw new ArgumentOutOfRangeException(nameof(order), "Order cannot be bigger than size of list");

            _productImages.Remove(productImage);
            _productImages.Add(new ProductImage(url, order));
            OrganizeProductImageOrder();
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
            if (_categories.Any(x => x.Id == category.Id))
                throw new InvalidOperationException($"Category with Id: {category.Id} already in product");

            _categories.Add(category);
            category.AddProduct(this);
        }
        public void RemoveCategory(int categoryId)
        {
            var category = _categories.FirstOrDefault(x => x.Id == categoryId);
            if (category is null)
                throw new KeyNotFoundException($"Category with Id: {categoryId} was not found");

            _categories.Remove(category);
            category.RemoveProduct(Id);
        }
    }
}
