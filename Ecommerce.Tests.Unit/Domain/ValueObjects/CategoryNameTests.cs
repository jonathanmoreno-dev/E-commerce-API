using System.Xml.Linq;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.ValueObjects
{
    public class CategoryNameTests
    {
        [Fact]
        public void ShouldBeEqualWhenNamesAreTheSame()
        {
            var name = new string('b', 16);

            var categoryName1 = new CategoryName(name);
            var categoryName2 = new CategoryName(name);

            Assert.Equal(categoryName1, categoryName2);
        }
        [Fact]
        public void ShouldNotBeEqualWhenNamesAreDifferent()
        {
            var name1 = new string('b', 16);
            var name2 = new string('c', 16);

            var categoryName1 = new CategoryName(name1);
            var categoryName2 = new CategoryName(name2);

            Assert.NotEqual(categoryName1, categoryName2);
        }
        [Fact]
        public void ShouldCreateValidCategoryName()
        {
            var name = new string('b', 16);

            var categoryName = new CategoryName(name);

            Assert.Equal(name, categoryName.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenNameIsNullOrWhiteSpace(string name)
        {
            Assert.Throws<DomainValidationException>(() => new CategoryName(name));
        }
        [Fact]
        public void ShouldTrimNameBeforeCreatingCategoryName()
        {
            var name = "Exemplo de Nome";
            var categoryName = new CategoryName($"    {name}    ");

            Assert.Equal(name, categoryName.Value);
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenNameIsMoreThan100Characters()
        {
            var name = new string('a', 101);

            Assert.Throws<DomainValidationException>(() => new CategoryName(name));
        }
        [Fact]
        public void ShouldCreateCategoryNameWithExactly100Characters()
        {
            var name = new string('a', 100);

            var categoryName = new CategoryName(name);

            Assert.Equal(name, categoryName.Value);
        }
    }
}
