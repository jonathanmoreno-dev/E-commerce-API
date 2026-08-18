using System.Xml.Linq;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.ValueObjects
{
    public record ProductName
    {
        public string Value { get; } = null!;
        private ProductName() { }
        public ProductName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException("Product name cannot be empty");

            value = value.Trim();

            if (value.Length > 255)
                throw new DomainValidationException("Product name cannot exceed 255 characters");

            Value = value;
        }
    }
}
