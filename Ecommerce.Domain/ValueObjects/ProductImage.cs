using Ecommerce.Domain.Exceptions;

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
                throw new DomainValidationException("ProductImageUrl cannot be empty");
            if (order <= 0)
                throw new DomainValidationException("Order must be greater than 0");

            Url = url;
            Order = order;
        }
    }
}
