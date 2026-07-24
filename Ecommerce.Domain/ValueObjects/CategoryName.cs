using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.ValueObjects
{
    public record CategoryName
    {
        public string Value { get; } = null!;

        private CategoryName() { }
        public CategoryName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException("Category name cannot be empty");
            if (value.Length > 100)
                throw new DomainValidationException("Category name cannot exceed 100 characters");

            Value = value;
        }
    }
}
