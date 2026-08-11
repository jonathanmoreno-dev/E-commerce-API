using Bogus;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Fakers;

public class CategoryFaker : Faker<Category>
{
    public CategoryFaker()
    {
        CustomInstantiator(f => new Category(
            new CategoryName(f.Commerce.Categories(1)[0]),
            new CategoryDescription(f.Commerce.ProductDescription())));
    }
    public static string GetCategoryImageUrl(string categoryName)
    {
        var faker = new Faker();
        return faker.Image.LoremFlickrUrl(800, 800, categoryName);
    }
    public static Category Create() => new CategoryFaker().Generate();
}