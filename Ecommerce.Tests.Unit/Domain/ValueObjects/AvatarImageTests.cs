using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.ValueObjects
{
    public class AvatarImageTests
    {
        [Fact]
        public void ShouldBeEqualWhenURLsAreTheSame()
        {
            var url = "https://example.com/avatar.png";

            var avatarImage1 = new AvatarImage(url);
            var avatarImage2 = new AvatarImage(url);

            Assert.Equal(avatarImage1, avatarImage2);
        }
        [Fact]
        public void ShouldNotBeEqualWhenURLsAreDifferent()
        {
            var url1 = "https://example.com/avatar.png";
            var url2 = "https://example.com/";

            var avatarImage1 = new AvatarImage(url1);
            var avatarImage2 = new AvatarImage(url2);

            Assert.NotEqual(avatarImage1, avatarImage2);
        }

        [Theory]
        [InlineData("http://example.com/avatar.png")]
        [InlineData("https://example.com/avatar.png")]
        public void ShouldCreateValidAvatarImage(string validUrl)
        {
            var avatarImage = new AvatarImage(validUrl);

            Assert.NotNull(avatarImage);
            Assert.Equal(validUrl, avatarImage.Url);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenUrlIsNullOrWhiteSpace(string url)
        {
            Assert.Throws<DomainValidationException>(() => new AvatarImage(url));
        }
        [Fact]
        public void ShouldTrimUrlBeforeCreatingAvatarImage()
        {
            var avatarImage = new AvatarImage("  https://example.com/avatar.png  ");

            Assert.Equal("https://example.com/avatar.png", avatarImage.Url);
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenUrlIsMoreThan2048Characters()
        {
            var baseUrl = "https://example.com/";
            var remainingLength = 2049 - baseUrl.Length;
            var url = baseUrl + new string('u', remainingLength);

            Assert.Throws<DomainValidationException>(() => new AvatarImage(url));
        }
        [Fact]
        public void ShouldCreateAvatarImageUrlWithExactly2048CharactersOfUrl()
        {
            var baseUrl = "https://example.com/";
            var remainingLength = 2048 - baseUrl.Length;
            var url = baseUrl + new string('u', remainingLength);

            var avatarImage = new AvatarImage(url);

            Assert.NotNull(avatarImage);
            Assert.Equal(url, avatarImage.Url);
        }
        [Theory]
        [InlineData("not-a-url")]
        [InlineData("example.com/avatar.png")]
        [InlineData("://invalid")]
        [InlineData("https://")]
        [InlineData("http://")]
        [InlineData("http:///example.com")]
        [InlineData("https://[invalid")]
        [InlineData("https://example .com")]
        public void ShouldThrowDomainValidationExceptionWhenUrlIsInvalid(string invalidUrl)
        {
            Assert.Throws<DomainValidationException>(() => new AvatarImage(invalidUrl));
        }
        [Theory]
        [InlineData("ftp://example.com/avatar.png")]
        [InlineData("file:///avatar.png")]
        [InlineData("mailto:avatar@example.com")]
        [InlineData("ws://example.com")]
        [InlineData("wss://example.com")]
        [InlineData("data:image/png;base64,abc")]
        public void ShouldThrowWhenUrlSchemeIsNotHttpOrHttps(string invalidUrl)
        {
            Assert.Throws<DomainValidationException>(() => new AvatarImage(invalidUrl));
        }
    }
}
