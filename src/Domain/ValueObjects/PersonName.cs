namespace E_commerce_API.src.Domain.ValueObjects
{
    public record PersonName
    {
        public string Value { get; } = null!;
        private PersonName() { }
        public PersonName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Person name cannot be empty", nameof(value));
            if (value.Length > 150)
                throw new ArgumentException("Person name exceed 150 characters", nameof(value));

            Value = value;
        }
    }
}
