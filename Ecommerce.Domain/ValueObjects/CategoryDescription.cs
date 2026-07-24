using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.ValueObjects
{
    public record CategoryDescription
    {
        public string Value { get; } = null!;
        private CategoryDescription() { }
        public CategoryDescription(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException("Category description cannot be empty");
            if (value.Length > 400)
                throw new DomainValidationException("Category description cannot exceed 400 characters");

            Value = value;
        }
    }
}
