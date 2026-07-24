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

            Url = url;
        }
    }
}
