namespace E_commerce_API.src.Domain.ValueObjects
{
    public record Quantity
    {
        public int Value { get; }

        private Quantity() { }
        public Quantity(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("Quantity cannot be negative");

            Value = value;
        }
        public Quantity Add(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException("Quantity must be greater than zero");

            return new Quantity(Value + value);
        }
        public Quantity Remove(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException("Quantity must be greater than zero");
            if(value > Value)
                throw new InvalidOperationException("Cannot remove more than available quantity");

            return new Quantity(Value - value);
        }
    }
}
