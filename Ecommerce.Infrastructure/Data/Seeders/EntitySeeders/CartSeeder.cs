using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;
using Ecommerce.Fakers;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Seeders.EntitySeeders
{
    public class CartSeeder
    {
        private readonly AppDbContext _appDbContext;
        public CartSeeder(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task SeedAsync()
        {
            var products = await _appDbContext.Products.ToListAsync();
            var users = await _appDbContext.Users.Where(x => x.Cart == null).ToListAsync();
            foreach (var user in users)
            {
                var cart = CartFaker.CreateRealCart(user.Id, products);
                _appDbContext.Carts.Add(cart);
            }
        }
    }
}
