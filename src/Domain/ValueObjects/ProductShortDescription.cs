namespace E_commerce_API.src.Domain.ValueObjects
{
    public class ProductShortDescription
    {
        public string Value { get; } = null!;
        private ProductShortDescription() { }
        public ProductShortDescription(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Product ShortDescription cannot be empty", nameof(value));
            if (value.Length > 400)
                throw new ArgumentException("Product ShortDescription cannot exceed 400 characters", nameof(value));

            Value = value;
        }
    }
}
