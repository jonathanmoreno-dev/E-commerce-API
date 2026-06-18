using System;
using System.Xml.Linq;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities
{
    public class Product
    {
        public int Id { get; private set; }
        public ProductName Name { get; private set; } = null!;
        public ProductShortDescription ShortDescription { get; private set; } = null!;
        public ProductLongDescription LongDescription { get; private set; } = null!;
        public Money Price { get; private set; } = null!;
        public Quantity Stock { get; private set; } = null!;
        private readonly List<ProductImage> _productImages = new();
        public IReadOnlyCollection<ProductImage> ProductImages => _productImages;
        private readonly List<Category> _categories = new();
        public IReadOnlyCollection<Category> Categories => _categories;

        private Product() { }
        public Product(ProductName name, ProductShortDescription shortDescription, ProductLongDescription longDescription, Money price, Quantity stock)
        {
            ArgumentNullException.ThrowIfNull(stock);

            ChangeName(name);
            ChangeShortDescription(shortDescription);
            ChangeLongDescription(longDescription);
            ChangePrice(price);
            Stock = stock;
        }
        public void ChangeName(ProductName name)
        {
            ArgumentNullException.ThrowIfNull(name);

            Name = name;
        }
        public void ChangeShortDescription(ProductShortDescription shortDescription)
        {
            ArgumentNullException.ThrowIfNull(shortDescription);

            ShortDescription = shortDescription;
        }
        public void ChangeLongDescription(ProductLongDescription longDescription)
        {
            ArgumentNullException.ThrowIfNull(longDescription);

            LongDescription = longDescription;
        }
        public void ChangePrice(Money price)
        {
            ArgumentNullException.ThrowIfNull(price);

            Price = price;
        }
        public void AddProductImage(ProductImage productImage)
        {
            ArgumentNullException.ThrowIfNull(productImage);

            _productImages.Add(productImage);
            OrganizeProductImageOrder();
        }
        public void RemoveProductImage(ProductImage productImage)
        {
            ArgumentNullException.ThrowIfNull(productImage);

            if (!_productImages.Contains(productImage))
                throw new KeyNotFoundException($"ProductImage was not found");

            _productImages.Remove(productImage);
            OrganizeProductImageOrder();
        }
        public void ChangeOrderProductImage(ProductImage productImage, int order)
        {
            ArgumentNullException.ThrowIfNull(productImage);

            if (!_productImages.Contains(productImage))
                throw new KeyNotFoundException($"ProductImage was not found");
            if (order > _productImages.Count)
                throw new ArgumentOutOfRangeException(nameof(order), "Order cannot be bigger than size of list");

            _productImages.Remove(productImage);
            _productImages.Insert(order - 1, new ProductImage(productImage.Url, order));
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
        public void IncreaseStock(int stock)
        {
            Stock = Stock.Add(stock);
        }
        public void DecreaseStock(int stock)
        {
            Stock = Stock.Remove(stock);
        }
        public void ChangeStock(Quantity stock)
        {
            Stock = stock;
        }
        public void AddCategory(Category category)
        {
            ArgumentNullException.ThrowIfNull(category);

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
