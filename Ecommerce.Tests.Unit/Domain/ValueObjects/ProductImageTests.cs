using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.ValueObjects
{
    public class ProductImageTests
    {
        [Fact]
        public void ShouldBeEqualWhenURLsAndOrdersAreTheSame()
        {
            var url = "https://example.com/product.png";

            var productImage1 = new ProductImage(url, 1);
            var productImage2 = new ProductImage(url, 1);

            Assert.Equal(productImage1, productImage2);
        }
        [Fact]
        public void ShouldNotBeEqualWhenURLsAreDifferent()
        {
            var url1 = "https://example.com/product.png";
            var url2 = "https://example.com/";

            var productImage1 = new ProductImage(url1, 1);
            var productImage2 = new ProductImage(url2, 1);

            Assert.NotEqual(productImage1, productImage2);
        }
        [Fact]
        public void ShouldNotBeEqualWhenOrdersAreDifferent()
        {
            var url = "https://example.com/product.png";

            var productImage1 = new ProductImage(url, 1);
            var productImage2 = new ProductImage(url, 2);

            Assert.NotEqual(productImage1, productImage2);
        }
        [Theory]
        [InlineData("http://example.com/product.png")]
        [InlineData("https://example.com/product.png")]
        public void ShouldCreateValidProductImage(string validUrl)
        {
            var productImage = new ProductImage(validUrl, 1);

            Assert.Equal(validUrl, productImage.Url);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenUrlIsNullOrWhiteSpace(string url)
        {
            Assert.Throws<DomainValidationException>(() => new ProductImage(url, 1));
        }
        [Theory]
        [InlineData(-2)]
        [InlineData(-1)]
        [InlineData(-0.001)]
        [InlineData(0)]
        public void ShouldThrowDomainValidationExceptionWhenOrderIsLessThanOne(int order)
        {
            Assert.Throws<DomainValidationException>(() => new ProductImage("https://example.com/product.png", order));
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ShouldCreateProductImageWithOrderMoreThanZero(int order)
        {
            var productImage = new ProductImage("https://example.com/product.png", order);

            Assert.Equal("https://example.com/product.png", productImage.Url);
        }
        [Fact]
        public void ShouldTrimUrlBeforeCreatingProductImage()
        {
            var productImage = new ProductImage("  https://example.com/product.png  ", 1);

            Assert.Equal("https://example.com/product.png", productImage.Url);
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenUrlIsMoreThan2048Characters()
        {
            var baseUrl = "https://example.com/";
            var remainingLength = 2049 - baseUrl.Length;
            var url = baseUrl + new string('u', remainingLength);

            Assert.Throws<DomainValidationException>(() => new ProductImage(url, 1));
        }
        [Fact]
        public void ShouldCreateProductImageWithExactly2048CharactersOfUrl()
        {
            var baseUrl = "https://example.com/";
            var remainingLength = 2048 - baseUrl.Length;
            var url = baseUrl + new string('u', remainingLength);

            var productImage = new ProductImage(url, 1);

            Assert.Equal(url, productImage.Url);
        }
        [Theory]
        [InlineData("not-a-url")]
        [InlineData("example.com/product.png")]
        [InlineData("://invalid")]
        [InlineData("https://")]
        [InlineData("http://")]
        [InlineData("http:///example.com")]
        [InlineData("https://[invalid")]
        [InlineData("https://example .com")]
        public void ShouldThrowDomainValidationExceptionWhenUrlIsInvalid(string invalidUrl)
        {
            Assert.Throws<DomainValidationException>(() => new ProductImage(invalidUrl, 1));
        }
        [Theory]
        [InlineData("ftp://example.com/product.png")]
        [InlineData("file:///product.png")]
        [InlineData("mailto:product@example.com")]
        [InlineData("ws://example.com")]
        [InlineData("wss://example.com")]
        [InlineData("data:image/png;base64,abc")]
        public void ShouldThrowWhenUrlSchemeIsNotHttpOrHttps(string invalidUrl)
        {
            Assert.Throws<DomainValidationException>(() => new ProductImage(invalidUrl, 1));
        }
    }
}
