using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.ValueObjects
{
    public record AvatarImage
    {
        public string Url { get; } = null!;
        private AvatarImage() { }
        public AvatarImage(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new DomainValidationException("AvatarImageUrl cannot be empty");

            url = url.Trim();

            if (url.Length > 2048)
                throw new DomainValidationException("URL cannot exceed 2048 characters");
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                throw new DomainValidationException("Invalid Url");
            if (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp)
                throw new DomainValidationException("Only HTTP and HTTPS URLs are allowed");

            Url = uri.AbsoluteUri;   
        }
    }
}
