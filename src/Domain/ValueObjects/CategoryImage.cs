namespace E_commerce_API.src.Domain.ValueObjects
{
    public record CategoryImage
    {
        public string Url { get; } = null!;
        private CategoryImage() { }
        public CategoryImage(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("CategoryImageUrl cannot be empty", nameof(url));

            Url = url;
        }
    }
}
