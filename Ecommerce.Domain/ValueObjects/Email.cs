using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; } = null!;
        private Email() { }
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException("Email cannot be empty");
            if (value.Length > 255)
                throw new DomainValidationException("Email cannot exceed 255 characters");

            Value = value;
        }
    }
}
