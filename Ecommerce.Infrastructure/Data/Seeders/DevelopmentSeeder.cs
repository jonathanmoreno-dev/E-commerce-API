namespace Ecommerce.Infrastructure.Data.Seeders.EntitySeeders
{
    public class DevelopmentSeeder
    {
        private readonly AppDbContext _appDbContext;
        private readonly CartSeeder _cartSeeder;
        private readonly CategorySeeder _categorySeeder;
        private readonly CheckoutSeeder _checkoutSeeder;
        private readonly OrderSeeder _orderSeeder;
        private readonly ProductSeeder _productSeeder;
        private readonly UserSeeder _userSeeder;
        public DevelopmentSeeder(AppDbContext appDbContext, CartSeeder cartSeeder, CategorySeeder categorySeeder, CheckoutSeeder checkoutSeeder, OrderSeeder orderSeeder, ProductSeeder productSeeder, UserSeeder userSeeder)
        {
            _appDbContext = appDbContext;
            _cartSeeder = cartSeeder;
            _categorySeeder = categorySeeder;
            _checkoutSeeder = checkoutSeeder;
            _orderSeeder = orderSeeder;
            _productSeeder = productSeeder;
            _userSeeder = userSeeder;
        }

        public async Task SeedAsync()
        {
            await using var transaction =
            await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                await _userSeeder.SeedAsync();
                await _appDbContext.SaveChangesAsync();

                await _categorySeeder.SeedAsync();
                await _appDbContext.SaveChangesAsync();

                await _productSeeder.SeedAsync();
                await _appDbContext.SaveChangesAsync();

                await _cartSeeder.SeedAsync();
                await _appDbContext.SaveChangesAsync();

                await _checkoutSeeder.SeedAsync();
                await _appDbContext.SaveChangesAsync();

                await _orderSeeder.SeedAsync();
                await _appDbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch 
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
