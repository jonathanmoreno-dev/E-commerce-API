using System.Xml.Linq;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; private set; }
        public CategoryName Name { get; private set; } = null!;
        public CategoryDescription Description { get; private set; } = null!;
        public CategoryImage? CategoryImage { get; private set; }
        private readonly List<Product> _products = new();
        public IReadOnlyCollection<Product> Products => _products;

        private Category() { }
        public Category(CategoryName name, CategoryDescription description)
        {
            Id = Guid.NewGuid();

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
    }
}
