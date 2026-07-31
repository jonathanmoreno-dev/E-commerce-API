using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.ValueObjects
{
    public record CategoryImage
    {
        public string Url { get; } = null!;
        private CategoryImage() { }
        public CategoryImage(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new DomainValidationException("CategoryImageUrl cannot be empty");

            url = url.Trim();

            if (url.Length > 2048)
                throw new DomainValidationException("URL cannot exceed 2048 characters");
            if(!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                throw new DomainValidationException("Invalid URL");
            if(uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
                throw new DomainValidationException("Only HTTP and HTTPS URLs are allowed");

            Url = uri.AbsoluteUri;
        }
    }
}
