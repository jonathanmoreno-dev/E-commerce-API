using Ecommerce.Domain.ValueObjects;
using Ecommerce.Fakers;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Seeders.EntitySeeders
{
    public class ProductSeeder
    {
        private readonly AppDbContext _appDbContext;
        public ProductSeeder(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task SeedAsync()
        {
            var categories = await _appDbContext.Categories.ToListAsync();

            foreach (var category in categories)
            {
                var quantityProductsToAdd = Random.Shared.Next(1, 20);
                for (int i = 0; i < quantityProductsToAdd; i++)
                {
                    var product = ProductFaker.Create();
                    var randomCategory = categories[Random.Shared.Next(0, categories.Count)];
                    product.AddCategory(randomCategory);
                    var quantityImagesToAdd = Random.Shared.Next(0, 5);
                    for (int j = 0; j < quantityImagesToAdd; j++)
                    {
                        product.AddProductImage(
                            new ProductImage(ProductFaker.GetProductImageUrl(product.Name.Value), j + 1)
                        );
                    }
                    _appDbContext.Products.Add(product);
                }
            }
        }
    }
}
