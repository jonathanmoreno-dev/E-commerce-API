using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;
using Ecommerce.Fakers;

namespace Ecommerce.Infrastructure.Data.Seeders.EntitySeeders
{
    public class CategorySeeder
    {
        private readonly AppDbContext _appDbContext;
        public CategorySeeder(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public Task SeedAsync()
        {
            var quantityCategoriesToAdd = Random.Shared.Next(10, 25);
            for (int i = 0; i < quantityCategoriesToAdd; i++)
            {
                var category = CategoryFaker.Create();
                category.ChangeCategoryImage(new CategoryImage(CategoryFaker.GetCategoryImageUrl(category.Name.Value)));
                _appDbContext.Categories.Add(category);
            }
            return Task.CompletedTask;
        }
    }
}
