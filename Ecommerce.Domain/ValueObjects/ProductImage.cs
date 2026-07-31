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
            url = url.Trim();
            if(url.Length > 2048)
                throw new DomainValidationException("URL cannot exceed 2048 characters");
            if(!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                throw new DomainValidationException("Invalid Url");
            if(uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp)
                throw new DomainValidationException("Only HTTP and HTTPS URLs are allowed");

            Url = uri.AbsoluteUri;
            Order = order;
        }
    }
}
