using System.Linq;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.Entities
{
    public class ProductTests
    {
        [Fact]
        public void ShouldCreateProductWithValidValues()
        {
            var name = new ProductName("Product");
            var shortDescription = new ProductShortDescription("Short description");
            var longDescription = new ProductLongDescription("Long product description");
            var price = new Money(99.90m);
            var stock = new Quantity(10);

            var product = new Product(name, shortDescription, longDescription, price, stock);

            Assert.NotEqual(Guid.Empty, product.Id);
            Assert.Equal(name, product.Name);
            Assert.Equal(shortDescription, product.ShortDescription);
            Assert.Equal(longDescription, product.LongDescription);
            Assert.Equal(price, product.Price);
            Assert.Equal(stock, product.Stock);
            Assert.Equal(new Quantity(0), product.ReservedStock);
            Assert.Empty(product.ProductImages);
            Assert.Empty(product.Categories);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenStockIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Product(
                new ProductName("Product"),
                new ProductShortDescription("Short description"),
                new ProductLongDescription("Long product description"),
                new Money(99.90m),
                null!));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Product(
                null!,
                new ProductShortDescription("Short description"),
                new ProductLongDescription("Long product description"),
                new Money(99.90m),
                new Quantity(10)));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenShortDescriptionIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Product(
                new ProductName("Product"),
                null!,
                new ProductLongDescription("Long product description"),
                new Money(99.90m),
                new Quantity(10)));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenLongDescriptionIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Product(
                new ProductName("Product"),
                new ProductShortDescription("Short description"),
                null!,
                new Money(99.90m),
                new Quantity(10)));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenPriceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Product(
                new ProductName("Product"),
                new ProductShortDescription("Short description"),
                new ProductLongDescription("Long product description"),
                null!,
                new Quantity(10)));
        }

        [Fact]
        public void ShouldChangeName()
        {
            var product = CreateProduct();
            var newName = new ProductName("Updated product");

            product.ChangeName(newName);

            Assert.Equal(newName, product.Name);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenChangingNameToNull()
        {
            var product = CreateProduct();

            Assert.Throws<ArgumentNullException>(() => product.ChangeName(null!));
        }

        [Fact]
        public void ShouldChangeShortDescription()
        {
            var product = CreateProduct();
            var newShortDescription = new ProductShortDescription("Updated short description");

            product.ChangeShortDescription(newShortDescription);

            Assert.Equal(newShortDescription, product.ShortDescription);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenChangingShortDescriptionToNull()
        {
            var product = CreateProduct();

            Assert.Throws<ArgumentNullException>(() => product.ChangeShortDescription(null!));
        }

        [Fact]
        public void ShouldChangeLongDescription()
        {
            var product = CreateProduct();
            var newLongDescription = new ProductLongDescription("Updated long product description");

            product.ChangeLongDescription(newLongDescription);

            Assert.Equal(newLongDescription, product.LongDescription);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenChangingLongDescriptionToNull()
        {
            var product = CreateProduct();

            Assert.Throws<ArgumentNullException>(() => product.ChangeLongDescription(null!));
        }

        [Fact]
        public void ShouldChangePrice()
        {
            var product = CreateProduct();
            var newPrice = new Money(149.90m);

            product.ChangePrice(newPrice);

            Assert.Equal(newPrice, product.Price);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenChangingPriceToNull()
        {
            var product = CreateProduct();

            Assert.Throws<ArgumentNullException>(() => product.ChangePrice(null!));
        }

        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenPriceIsZero()
        {
            var product = CreateProduct();

            Assert.Throws<DomainValidationException>(() => product.ChangePrice(new Money(0m)));
        }

        [Fact]
        public void ShouldAddProductImage()
        {
            var product = CreateProduct();
            var productImage = new ProductImage("https://example.com/image.png", 2);

            product.AddProductImage(productImage);

            var addedImage = Assert.Single(product.ProductImages);

            Assert.Equal(productImage.Url, addedImage.Url);
            Assert.Equal(1, addedImage.Order);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenAddingProductImageNull()
        {
            var product = CreateProduct();

            Assert.Throws<ArgumentNullException>(() => product.AddProductImage(null!));
        }

        [Fact]
        public void ShouldRemoveProductImage()
        {
            var product = CreateProductWithImages();
            var productImage = product.ProductImages.Last();

            product.RemoveProductImage(productImage);

            var remainingImage = Assert.Single(product.ProductImages);

            Assert.Equal("https://example.com/first.png", remainingImage.Url);
            Assert.Equal(1, remainingImage.Order);
        }

        [Fact]
        public void ShouldThrowNotFoundExceptionWhenRemovingProductImageThatDoesNotExist()
        {
            var product = CreateProduct();

            Assert.Throws<NotFoundException>(() => product.RemoveProductImage(
                new ProductImage("https://example.com/image.png", 1)));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenRemovingProductImageNull()
        {
            var product = CreateProduct();

            Assert.Throws<ArgumentNullException>(() => product.RemoveProductImage(null!));
        }

        [Fact]
        public void ShouldChangeProductImageUrl()
        {
            var product = CreateProductWithImages();
            var productImage = product.ProductImages.First();
            var newUrl = "https://example.com/updated-image.png";

            product.ChangeUrlProductImage(productImage, newUrl);

            var changedImage = product.ProductImages.First(x => x.Url == newUrl);

            Assert.Equal(newUrl, changedImage.Url);
            Assert.Equal(1, changedImage.Order);
        }

        [Fact]
        public void ShouldThrowNotFoundExceptionWhenChangingUrlOfProductImageThatDoesNotExist()
        {
            var product = CreateProduct();

            Assert.Throws<NotFoundException>(() => product.ChangeUrlProductImage(
                new ProductImage("https://example.com/image.png", 1),
                "https://example.com/updated-image.png"));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenChangingUrlOfProductImageNull()
        {
            var product = CreateProduct();

            Assert.Throws<ArgumentNullException>(() => product.ChangeUrlProductImage(
                null!,
                "https://example.com/updated-image.png"));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenChangingOrderOfProductImageNull()
        {
            var product = CreateProduct();

            Assert.Throws<ArgumentNullException>(() => product.ChangeOrderProductImage(null!, 1));
        }

        [Fact]
        public void ShouldChangeProductImageOrder()
        {
            var product = CreateProductWithImages();
            var productImage = product.ProductImages.First();

            product.ChangeOrderProductImage(productImage, 2);

            var images = product.ProductImages.ToList();

            Assert.Equal("https://example.com/second.png", images[0].Url);
            Assert.Equal(1, images[0].Order);
            Assert.Equal("https://example.com/first.png", images[1].Url);
            Assert.Equal(2, images[1].Order);
        }

        [Fact]
        public void ShouldThrowNotFoundExceptionWhenChangingOrderOfProductImageThatDoesNotExist()
        {
            var product = CreateProduct();

            Assert.Throws<NotFoundException>(() => product.ChangeOrderProductImage(
                new ProductImage("https://example.com/image.png", 1),
                1));
        }

        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenNewImageOrderIsGreaterThanImageCount()
        {
            var product = CreateProductWithImages();
            var productImage = product.ProductImages.First();

            Assert.Throws<DomainValidationException>(() => product.ChangeOrderProductImage(productImage, 3));
        }

        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenNewImageOrderIsZero()
        {
            var product = CreateProductWithImages();
            var productImage = product.ProductImages.First();

            Assert.Throws<DomainValidationException>(() => product.ChangeOrderProductImage(productImage, 0));
        }

        [Fact]
        public void ShouldChangeStock()
        {
            var product = CreateProduct();
            var newStock = new Quantity(20);

            product.ChangeStock(newStock);

            Assert.Equal(newStock, product.Stock);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenStockIsLessThanReservedStock()
        {
            var product = CreateProduct();
            product.ReserveStock(new Quantity(5));

            Assert.Throws<BusinessRuleException>(() => product.ChangeStock(new Quantity(4)));
        }

        [Fact]
        public void ShouldReserveStock()
        {
            var product = CreateProduct();

            product.ReserveStock(new Quantity(3));

            Assert.Equal(new Quantity(3), product.ReservedStock);
            Assert.Equal(new Quantity(10), product.Stock);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenReservingMoreStockThanAvailable()
        {
            var product = CreateProduct();

            Assert.Throws<BusinessRuleException>(() => product.ReserveStock(new Quantity(11)));
        }

        [Fact]
        public void ShouldConfirmStockReservation()
        {
            var product = CreateProduct();
            product.ReserveStock(new Quantity(3));

            product.ConfirmStockReservation(new Quantity(3));

            Assert.Equal(new Quantity(0), product.ReservedStock);
            Assert.Equal(new Quantity(7), product.Stock);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenConfirmingMoreStockThanReserved()
        {
            var product = CreateProduct();
            product.ReserveStock(new Quantity(2));

            Assert.Throws<BusinessRuleException>(() => product.ConfirmStockReservation(new Quantity(3)));
        }

        [Fact]
        public void ShouldCancelStockReservation()
        {
            var product = CreateProduct();
            product.ReserveStock(new Quantity(3));

            product.CancelStockReservation(new Quantity(2));

            Assert.Equal(new Quantity(1), product.ReservedStock);
            Assert.Equal(new Quantity(10), product.Stock);
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenCancelingMoreStockThanReserved()
        {
            var product = CreateProduct();
            product.ReserveStock(new Quantity(2));

            Assert.Throws<BusinessRuleException>(() => product.CancelStockReservation(new Quantity(3)));
        }

        [Fact]
        public void ShouldNotThrowWhenStockIsAvailable()
        {
            var product = CreateProduct();
            product.ReserveStock(new Quantity(3));

            product.CheckAvailability(new Quantity(7));
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenStockIsUnavailable()
        {
            var product = CreateProduct();
            product.ReserveStock(new Quantity(3));

            Assert.Throws<BusinessRuleException>(() => product.CheckAvailability(new Quantity(8)));
        }

        [Fact]
        public void ShouldAddCategory()
        {
            var product = CreateProduct();
            var category = CreateCategory();

            product.AddCategory(category);

            var addedCategory = Assert.Single(product.Categories);

            Assert.Equal(category, addedCategory);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenAddingCategoryNull()
        {
            var product = CreateProduct();

            Assert.Throws<ArgumentNullException>(() => product.AddCategory(null!));
        }

        [Fact]
        public void ShouldThrowConflictExceptionWhenAddingTheSameCategoryTwice()
        {
            var product = CreateProduct();
            var category = CreateCategory();
            product.AddCategory(category);

            Assert.Throws<ConflictException>(() => product.AddCategory(category));
        }

        [Fact]
        public void ShouldRemoveCategory()
        {
            var product = CreateProduct();
            var category = CreateCategory();
            product.AddCategory(category);

            product.RemoveCategory(category.Id);

            Assert.Empty(product.Categories);
        }

        [Fact]
        public void ShouldThrowNotFoundExceptionWhenRemovingCategoryThatDoesNotExist()
        {
            var product = CreateProduct();

            Assert.Throws<NotFoundException>(() => product.RemoveCategory(Guid.NewGuid()));
        }

        private static Product CreateProduct()
        {
            return new Product(
                new ProductName("Product"),
                new ProductShortDescription("Short description"),
                new ProductLongDescription("Long product description"),
                new Money(99.90m),
                new Quantity(10));
        }

        private static Product CreateProductWithImages()
        {
            var product = CreateProduct();

            product.AddProductImage(new ProductImage("https://example.com/first.png", 1));
            product.AddProductImage(new ProductImage("https://example.com/second.png", 2));

            return product;
        }

        private static Category CreateCategory()
        {
            return new Category(new CategoryName("Category"), new CategoryDescription("Category description"));
        }
    }
}
