namespace E_commerce_API.src.Domain.ValueObjects
{
    public record PhoneNumber
    {
        public string Value { get; } = null!;
        private PhoneNumber() { }
        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Phone number cannot be empty", nameof(value));
            if (value.Length > 50)
                throw new ArgumentException("Phone number exceed 50 characters", nameof(value));

            Value = value;
        }
    }
}
