using System;
using System.Xml.Linq;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public ProductName Name { get; private set; } = null!;
        public ProductShortDescription ShortDescription { get; private set; } = null!;
        public ProductLongDescription LongDescription { get; private set; } = null!;
        public Money Price { get; private set; } = null!;
        public Quantity Stock { get; private set; } = null!;
        public Quantity ReservedStock { get; private set; } = null!;
        private readonly List<ProductImage> _productImages = new();
        public IReadOnlyCollection<ProductImage> ProductImages => _productImages;
        private readonly List<Category> _categories = new();
        public IReadOnlyCollection<Category> Categories => _categories;

        private Product() { }
        public Product(ProductName name, ProductShortDescription shortDescription, ProductLongDescription longDescription, Money price, Quantity stock)
        {
            Id = Guid.NewGuid();

            ArgumentNullException.ThrowIfNull(stock);

            ChangeName(name);
            ChangeShortDescription(shortDescription);
            ChangeLongDescription(longDescription);
            ChangePrice(price);
            Stock = stock;
            ReservedStock = new Quantity(0);
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
            if(price.Value <= 0)
                throw new DomainValidationException("Money must be greater than zero");

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
                throw new NotFoundException($"ProductImage was not found");

            _productImages.Remove(productImage);
            OrganizeProductImageOrder();
        }
        public void ChangeUrlProductImage(ProductImage productImage, string newUrl)
        {
            ArgumentNullException.ThrowIfNull(productImage);

            if (!_productImages.Contains(productImage))
                throw new NotFoundException($"ProductImage was not found");

            _productImages.Remove(productImage);
            _productImages.Add(new ProductImage(newUrl, productImage.Order));
            OrganizeProductImageOrder();
        }
        public void ChangeOrderProductImage(ProductImage productImage, int newOrder)
        {
            ArgumentNullException.ThrowIfNull(productImage);

            if (!_productImages.Contains(productImage))
                throw new NotFoundException($"ProductImage was not found");
            if (newOrder > _productImages.Count)
                throw new DomainValidationException("Order cannot be bigger than size of list");
            if (newOrder <= 0)
                throw new DomainValidationException("NewOrder must be greater than 0");

            _productImages.Remove(productImage);
            _productImages.Insert(newOrder - 1, new ProductImage(productImage.Url, newOrder));
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
        public void IncreaseStock(Quantity quantity)
        {
            Stock = Stock.Add(quantity.Value);
        }
        public void DecreaseStock(Quantity quantity)
        {
            if ((Stock.Value - quantity.Value) < ReservedStock.Value)
                throw new BusinessRuleException("Stock cannot be less than reserved stock");

            Stock = Stock.Remove(quantity.Value);
        }
        public void ChangeStock(Quantity quantity)
        {
            if(quantity.Value < ReservedStock.Value)
                throw new BusinessRuleException("Stock cannot be less than reserved stock");

            Stock = quantity;
        }
        public void ReserveStock(Quantity quantity)
        {
            var availableStock = Stock.Value - quantity.Value;
            if (availableStock < quantity.Value)
                throw new BusinessRuleException("Insufficient stock");

            ReservedStock = ReservedStock.Add(quantity.Value);
        }
        public void ConfirmStockReservation(Quantity quantity)
        {
            ReservedStock = ReservedStock.Remove(quantity.Value);
            DecreaseStock(quantity);
        }
        public void CancelStockReservation(Quantity quantity)
        {
            ReservedStock = ReservedStock.Remove(quantity.Value);
        }
        public void AddCategory(Category category)
        {
            ArgumentNullException.ThrowIfNull(category);

            if (_categories.Any(x => x.Id == category.Id))
                throw new ConflictException("Category already in Product", $"Category with Id: {category.Id} already in product");

            _categories.Add(category);
            category.AddProduct(this);
        }
        public void RemoveCategory(Guid categoryId)
        {
            var category = _categories.FirstOrDefault(x => x.Id == categoryId);
            if (category is null)
                throw new NotFoundException("Category", $"Category with Id: {categoryId} was not found");

            _categories.Remove(category);
            category.RemoveProduct(Id);
        }
    }
}
