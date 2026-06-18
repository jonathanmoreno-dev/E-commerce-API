namespace Ecommerce.Domain.ValueObjects
{
    public record ProductImage
    {
        public string Url { get; } = null!;
        public int Order { get; }
        private ProductImage() { }
        public ProductImage(string url, int order)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("ProductImageUrl cannot be empty", nameof(url));
            if (order <= 0)
                throw new ArgumentOutOfRangeException(nameof(order), "Order must be greater than 0");

            Url = url;
            Order = order;
        }
    }
}
