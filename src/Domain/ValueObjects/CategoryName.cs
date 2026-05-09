namespace E_commerce_API.src.Domain.ValueObjects
{
    public record CategoryName
    {
        public string Name { get; } = "";

        private CategoryName() { }
        public CategoryName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category's name cannot be empty");
            if (name.Length > 100)
                throw new ArgumentException("Category's name cannot exceed 100 characters");

            Name = name;
        }
    }
}
