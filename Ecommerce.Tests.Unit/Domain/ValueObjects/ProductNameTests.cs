using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.ValueObjects
{
    public class ProductNameTests
    {
        [Fact]
        public void ShouldBeEqualWhenNamesAreTheSame()
        {
            var name = new string('b', 16);

            var productName1 = new ProductName(name);
            var productName2 = new ProductName(name);

            Assert.Equal(productName1, productName2);
        }
        [Fact]
        public void ShouldNotBeEqualWhenNamesAreDifferent()
        {
            var name1 = new string('b', 16);
            var name2 = new string('c', 16);

            var productName1 = new ProductName(name1);
            var productName2 = new ProductName(name2);

            Assert.NotEqual(productName1, productName2);
        }
        [Fact]
        public void ShouldCreateValidCategoryName()
        {
            var name = new string('b', 16);

            var productName = new ProductName(name);

            Assert.Equal(name, productName.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenNameIsNullOrWhiteSpace(string name)
        {
            Assert.Throws<DomainValidationException>(() => new ProductName(name));
        }
        [Fact]
        public void ShouldTrimNameBeforeCreatingProductName()
        {
            var name = "Exemplo de Nome";
            var productName = new ProductName($"    {name}    ");

            Assert.Equal(name, productName.Value);
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenNameIsMoreThan255Characters()
        {
            var name = new string('a', 256);

            Assert.Throws<DomainValidationException>(() => new ProductName(name));
        }
        [Fact]
        public void ShouldCreateProductNameWithExactly255Characters()
        {
            var name = new string('a', 255);

            var productName = new ProductName(name);

            Assert.Equal(name, productName.Value);
        }
    }
}
