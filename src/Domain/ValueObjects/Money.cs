namespace E_commerce_API.src.Domain.ValueObjects
{
    public record Money
    {
        public decimal Value { get; }

        private Money() { }
        public Money(decimal value)
        {
            if (value <= 0)
                throw new ArgumentException("Money must be greater than zero");

            Value = value;
        }
    }
}
