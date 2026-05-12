namespace E_commerce_API.src.Domain.ValueObjects
{
    public record CategoryDescription
    {
        public string Value { get; } = null!;
        private CategoryDescription() { }
        public CategoryDescription(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Category description cannot be empty", nameof(value));
            if (value.Length > 400)
                throw new ArgumentException("Category description cannot exceed 400 characters", nameof(value));

            Value = value;
        }
    }
}
