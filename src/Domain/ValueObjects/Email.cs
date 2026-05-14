namespace E_commerce_API.src.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; } = null!;
        private Email() { }
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty", nameof(value));
            if (value.Length > 255)
                throw new ArgumentException("Email cannot exceed 255 characters", nameof(value));

            Value = value;
        }
    }
}
