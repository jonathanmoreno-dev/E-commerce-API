namespace E_commerce_API.src.Domain.ValueObjects
{
    public record CategoryName
    {
        public string Value { get; } = null!;

        private CategoryName() { }
        public CategoryName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Category's name cannot be empty");
            if (value.Length > 100)
                throw new ArgumentException("Category's name cannot exceed 100 characters");

            Value = value;
        }
    }
}
