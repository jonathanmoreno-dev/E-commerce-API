using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class Category
    {
        public int CategoryId { get; private set; }
        public CategoryName Name { get; private set; } = null!;
        public string Description { get; private set; } = "";

        private readonly List<Product> _products = new();
        public IReadOnlyCollection<Product> Products => _products;

        private Category() { }
        public Category(string name)
        {
            ChangeName(name);
        }
        public void ChangeName(string name)
        {
            Name = new CategoryName(name);
        }
        public void SetDescription(string description)
        {
            Description = description;
        }
        public void AddProduct(Product product)
        {
            if (product is null)
                throw new ArgumentNullException("Product cannot be null");
            if (_products.Any(x => x.ProductId == product.ProductId))
                throw new ArgumentException($"Product with this Id: {product.ProductId} already in category");

            _products.Add(product);
        }
        public void RemoveProduct(int productId)
        {
            var product = _products.FirstOrDefault(x => x.ProductId == productId);
            if(product is null)
                throw new ArgumentException($"Product not found with this Id: {productId}");

            _products.Remove(product);
        }
    }
}
