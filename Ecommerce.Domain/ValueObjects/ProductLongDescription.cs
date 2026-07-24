using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.ValueObjects
{
    public record ProductLongDescription
    {
        public string Value { get; } = null!;
        private ProductLongDescription() { }
        public ProductLongDescription(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException("Product LongDescription cannot be empty");

            Value = value;
        }
    }
}
