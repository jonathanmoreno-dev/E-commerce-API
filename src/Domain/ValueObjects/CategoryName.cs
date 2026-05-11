namespace E_commerce_API.src.Domain.ValueObjects
{
    public record CategoryName
    {
        public string Value { get; } = null!;

        private CategoryName() { }
        public CategoryName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Category name cannot be empty", nameof(value));
            if (value.Length > 100)
                throw new ArgumentException("Category name cannot exceed 100 characters", nameof(value));

            Value = value;
        }
    }
}
