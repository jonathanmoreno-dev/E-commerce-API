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

            Url = url;   
        }
    }
}
