namespace E_commerce_API.src.Domain.ValueObjects
{
    public record CategoryDescription
    {
        public string Value { get; } = null!;
        private CategoryDescription() { }
        public CategoryDescription(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Category description cannot be empty");
            if (value.Length > 400)
                throw new ArgumentException("Category description cannot exceed 100 characters");

            Value = value;
        }
    }
}
