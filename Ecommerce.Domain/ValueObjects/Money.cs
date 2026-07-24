using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.ValueObjects
{
    public record Money
    {
        public decimal Value { get; }

        private Money() { }
        public Money(decimal value)
        {
            if (value < 0)
                throw new DomainValidationException("Money cannot be negative");

            Value = value;
        }
    }
}
