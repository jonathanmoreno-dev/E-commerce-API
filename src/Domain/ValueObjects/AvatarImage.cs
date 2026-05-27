namespace E_commerce_API.src.Domain.ValueObjects
{
    public record AvatarImage
    {
        public string Url { get; } = null!;
        private AvatarImage() { }
        public AvatarImage(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("AvatarImageUrl cannot be empty", nameof(url));

            Url = url;   
        }
    }
}
