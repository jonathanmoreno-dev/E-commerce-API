using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.ValueObjects
{
    public class CategoryDescriptionTests
    {
        [Fact]
        public void ShouldBeEqualWhenDescriptionsAreTheSame()
        {
            var description = new string('b', 214);

            var categoryDescription1 = new CategoryDescription(description);
            var categoryDescription2 = new CategoryDescription(description);

            Assert.Equal(categoryDescription1, categoryDescription2);
        }
        [Fact]
        public void ShouldNotBeEqualWhenDescriptionsAreDifferent()
        {
            var description1 = new string('b', 214);
            var description2 = new string('c', 214);

            var categoryDescription1 = new CategoryDescription(description1);
            var categoryDescription2 = new CategoryDescription(description2);

            Assert.NotEqual(categoryDescription1, categoryDescription2);
        }
        [Fact]
        public void ShouldCreateValidCategoryDescription()
        {
            var description = new string('b', 214);

            var categoryDescription = new CategoryDescription(description);

            Assert.Equal(description, categoryDescription.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenDescriptionIsNullOrWhiteSpace(string description)
        {
            Assert.Throws<DomainValidationException>(() => new CategoryDescription(description));
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenDescriptionIsMoreThan400Characters()
        {
            var description = new string('a', 401);

            Assert.Throws<DomainValidationException>(() => new CategoryDescription(description));
        }
        [Fact]
        public void ShouldCreateCategoryDescriptionWithExactly400Characters()
        {
            var description = new string('a', 400);

            var categoryDescription = new CategoryDescription(description);

            Assert.Equal(description, categoryDescription.Value);
        }
    }
}
