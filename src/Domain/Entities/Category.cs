using System.Xml.Linq;
using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class Category
    {
        public int Id { get; private set; }
        public CategoryName Name { get; private set; } = null!;
        public CategoryDescription Description { get; private set; } = null!;
        public CategoryImage? CategoryImage { get; private set; }
        private readonly List<Product> _products = new();
        public IReadOnlyCollection<Product> Products => _products;

        private Category() { }
        public Category(CategoryName name, CategoryDescription description)
        {
            ChangeName(name);
            ChangeDescription(description);
        }
        public void ChangeName(CategoryName name)
        {
            ArgumentNullException.ThrowIfNull(name);

            Name = name;
        }
        public void ChangeDescription(CategoryDescription description)
        {
            ArgumentNullException.ThrowIfNull(description);

            Description = description;
        }
        public void ChangeCategoryImage(CategoryImage categoryImage)
        {
            ArgumentNullException.ThrowIfNull(categoryImage);

            CategoryImage = categoryImage;
        }
        public void AddProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);

            if (_products.Any(x => x.Id == product.Id))
                throw new InvalidOperationException($"Product with Id: {product.Id} already in category");

            _products.Add(product);
        }
        public void RemoveProduct(int productId)
        {
            var product = _products.FirstOrDefault(x => x.Id == productId);
            if(product is null)
                throw new KeyNotFoundException($"Product with Id: {productId} was not found");

            _products.Remove(product);
        }
    }
}
