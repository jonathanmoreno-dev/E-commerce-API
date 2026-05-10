using System.Xml.Linq;

namespace E_commerce_API.src.Domain.ValueObjects
{
    public class ProductName
    {
        public string Value { get; } = null!;
        public ProductName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Product name cannot be empty");
            if (value.Length > 255)
                throw new ArgumentOutOfRangeException("Product name cannot exceed 255 characters");

            Value = value;
        }
    }
}
