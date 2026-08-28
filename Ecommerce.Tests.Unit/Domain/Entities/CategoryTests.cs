using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.Entities
{
    public class CategoryTests
    {
        [Fact]
        public void ShouldCreateCategoryWithValidValues()
        {
            var name = new CategoryName("Electronics");
            var description = new CategoryDescription("Electronic products");

            var category = new Category(name, description);

            Assert.NotEqual(Guid.Empty, category.Id);
            Assert.Equal(name, category.Name);
            Assert.Equal(description, category.Description);
            Assert.Null(category.CategoryImage);
            Assert.Empty(category.Products);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Category(null!, new CategoryDescription("Electronic products")));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenDescriptionIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Category(new CategoryName("Electronics"), null!));
        }

        [Fact]
        public void ShouldChangeName()
        {
            var category = new Category(new CategoryName("Electronics"), new CategoryDescription("Electronic products"));
            var newName = new CategoryName("Books");

            category.ChangeName(newName);

            Assert.Equal(newName, category.Name);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenChangingNameToNull()
        {
            var category = new Category(new CategoryName("Electronics"), new CategoryDescription("Electronic products"));

            Assert.Throws<ArgumentNullException>(() => category.ChangeName(null!));
        }

        [Fact]
        public void ShouldChangeDescription()
        {
            var category = new Category(new CategoryName("Electronics"), new CategoryDescription("Electronic products"));
            var newDescription = new CategoryDescription("Updated description");

            category.ChangeDescription(newDescription);

            Assert.Equal(newDescription, category.Description);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenChangingDescriptionToNull()
        {
            var category = new Category(new CategoryName("Electronics"), new CategoryDescription("Electronic products"));

            Assert.Throws<ArgumentNullException>(() => category.ChangeDescription(null!));
        }

        [Fact]
        public void ShouldChangeCategoryImage()
        {
            var category = new Category(new CategoryName("Electronics"), new CategoryDescription("Electronic products"));
            var categoryImage = new CategoryImage("https://example.com/category.png");

            category.ChangeCategoryImage(categoryImage);

            Assert.Equal(categoryImage, category.CategoryImage);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenChangingCategoryImageToNull()
        {
            var category = new Category(new CategoryName("Electronics"), new CategoryDescription("Electronic products"));

            Assert.Throws<ArgumentNullException>(() => category.ChangeCategoryImage(null!));
        }
    }
}