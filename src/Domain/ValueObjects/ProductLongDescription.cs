namespace E_commerce_API.src.Domain.ValueObjects
{
    public record ProductLongDescription
    {
        public string Value { get; } = null!;
        private ProductLongDescription() { }
        public ProductLongDescription(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Product LongDescription cannot be empty", nameof(value));

            Value = value;
        }
    }
}
