using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.ValueObjects
{
    public record ProductShortDescription
    {
        public string Value { get; } = null!;
        private ProductShortDescription() { }
        public ProductShortDescription(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException("Product ShortDescription cannot be empty");
            if (value.Length > 400)
                throw new DomainValidationException("Product ShortDescription cannot exceed 400 characters");

            Value = value;
        }
    }
}
