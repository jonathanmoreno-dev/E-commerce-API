using System;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.ValueObjects
{
    public class CategoryImageTests
    {
        [Fact]
        public void ShouldBeEqualWhenURLsAreTheSame()
        {
            var url = "https://example.com/category.png";

            var categoryImage1 = new CategoryImage(url);
            var categoryImage2 = new CategoryImage(url);

            Assert.Equal(categoryImage1, categoryImage2);
        }
        [Fact]
        public void ShouldNotBeEqualWhenURLsAreDifferent()
        {
            var url1 = "https://example.com/category.png";
            var url2 = "https://example.com/";

            var categoryImage1 = new CategoryImage(url1);
            var categoryImage2 = new CategoryImage(url2);

            Assert.NotEqual(categoryImage1, categoryImage2);
        }
        [Theory]
        [InlineData("http://example.com/category.png")]
        [InlineData("https://example.com/category.png")]
        public void ShouldCreateValidCategoryImage(string validUrl)
        {
            var categoryImage = new CategoryImage(validUrl);

            Assert.Equal(validUrl, categoryImage.Url);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenUrlIsNullOrWhiteSpace(string url)
        {
            Assert.Throws<DomainValidationException>(() => new CategoryImage(url));
        }
        [Fact]
        public void ShouldTrimUrlBeforeCreatingCategoryImage()
        {
            var categoryImage = new CategoryImage("  https://example.com/category.png  ");

            Assert.Equal("https://example.com/category.png", categoryImage.Url);
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenUrlIsMoreThan2048Characters()
        {
            var baseUrl = "https://example.com/";
            var remainingLength = 2049 - baseUrl.Length;
            var url = baseUrl + new string('u', remainingLength);

            Assert.Throws<DomainValidationException>(() => new CategoryImage(url));
        }
        [Fact]
        public void ShouldCreateCategoryImageWithExactly2048CharactersOfUrl()
        {
            var baseUrl = "https://example.com/";
            var remainingLength = 2048 - baseUrl.Length;
            var url = baseUrl + new string('u', remainingLength);

            var categoryImage = new CategoryImage(url);

            Assert.Equal(url, categoryImage.Url);
        }
        [Theory]
        [InlineData("not-a-url")]
        [InlineData("example.com/category.png")]
        [InlineData("://invalid")]
        [InlineData("https://")]
        [InlineData("http://")]
        [InlineData("http:///example.com")]
        [InlineData("https://[invalid")]
        [InlineData("https://example .com")]
        public void ShouldThrowDomainValidationExceptionWhenUrlIsInvalid(string invalidUrl)
        {
            Assert.Throws<DomainValidationException>(() => new CategoryImage(invalidUrl));
        }
        [Theory]
        [InlineData("ftp://example.com/category.png")]
        [InlineData("file:///category.png")]
        [InlineData("mailto:category@example.com")]
        [InlineData("ws://example.com")]
        [InlineData("wss://example.com")]
        [InlineData("data:image/png;base64,abc")]
        public void ShouldThrowWhenUrlSchemeIsNotHttpOrHttps(string invalidUrl)
        {
            Assert.Throws<DomainValidationException>(() => new CategoryImage(invalidUrl));
        }
    }
}
