using Bogus;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Fakers;

public class ProductFaker : Faker<Product>
{
    public ProductFaker()
    {
        CustomInstantiator(f => new Product(
            new ProductName(f.Commerce.Product()),
            new ProductShortDescription(f.Commerce.ProductName()),
            new ProductLongDescription(f.Commerce.ProductDescription()),
            new Money(f.Random.Decimal(1, 2000)),
            new Quantity(f.Random.Int(5, 12))));
    }
    public static string GetProductImageUrl(string productName)
    {
        var faker = new Faker();
        return faker.Image.LoremFlickrUrl(800, 800, productName);
    }
    public static Product Create() => new ProductFaker().Generate();
}