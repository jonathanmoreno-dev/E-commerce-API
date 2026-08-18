using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.ValueObjects
{
    public record PersonName
    {
        public string Value { get; } = null!;
        private PersonName() { }
        public PersonName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException("Person name cannot be empty");

            value = value.Trim();
            if (value.Length > 150)
                throw new DomainValidationException("Person name exceed 150 characters");

            Value = value;
        }
    }
}
