using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.ValueObjects
{
    public record PhoneNumber
    {
        public string Value { get; } = null!;
        private PhoneNumber() { }
        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException("Phone number cannot be empty");
            if (value.Length > 50)
                throw new DomainValidationException("Phone number exceed 50 characters");

            Value = value;
        }
    }
}
