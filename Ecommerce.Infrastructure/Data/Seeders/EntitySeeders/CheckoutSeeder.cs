using Ecommerce.Domain.Enums;
using Ecommerce.Fakers;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Seeders.EntitySeeders
{
    public class CheckoutSeeder
    {
        private readonly AppDbContext _appDbContext;
        public CheckoutSeeder(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task SeedAsync()
        {
            var users = await _appDbContext.Users.ToListAsync();
            var products = await _appDbContext.Products.ToListAsync();
            var dictionaryProducts = products.ToDictionary(x => x.Id);
            foreach (var user in users)
            {
                var quantityCheckoutsToAdd = Random.Shared.Next(1, 3);
                for (int i = 0; i < quantityCheckoutsToAdd; i++)
                {
                    var checkout = CheckoutFaker.CreateRealCheckout(user, products);
                    if (checkout is null)
                        continue;
                    var shouldCreatePayment = Random.Shared.Next(0, 3) == 0; // 33% of chance

                    foreach (var item in checkout.CheckoutItems)
                    {
                        if (dictionaryProducts.TryGetValue(item.ProductId, out var product))
                            product.ReserveStock(item.Quantity);
                    }
                    if (shouldCreatePayment)
                    {
                        var paymentMethod = Random.Shared.Next(1, 4);
                        checkout.ChangePaymentMethod((PaymentMethod)paymentMethod);
                        checkout.CreatePayment();
                        var chanceAuthorizePayment = Random.Shared.Next(0, 4); // 25% of chance
                        if (chanceAuthorizePayment == 1)
                        {
                            checkout.AuthorizePayment();
                            var shouldCompletePayment = Random.Shared.Next(0, 2) == 0; // 50% of chance
                            if (shouldCompletePayment)
                            {
                                checkout.CompletePayment();
                                foreach (var item in checkout.CheckoutItems)
                                {
                                    if (dictionaryProducts.TryGetValue(item.ProductId, out var product))
                                        product.ConfirmStockReservation(item.Quantity);
                                }
                            }
                        }
                        else
                        {
                            foreach (var item in checkout.CheckoutItems)
                            {
                                if (dictionaryProducts.TryGetValue(item.ProductId, out var product))
                                    product.CancelStockReservation(item.Quantity);
                            }
                            var shouldFailPayment = Random.Shared.Next(0, 10) == 0; // 10% of chance
                            var shouldAbandonPayment = Random.Shared.Next(0, 5) == 0; // 20% of chance
                            if (shouldFailPayment)
                                checkout.FailPayment();
                            else if (shouldAbandonPayment)
                                checkout.AbandonPayment();
                            else
                                checkout.CancelPayment();
                        }
                    }
                    _appDbContext.Checkouts.Add(checkout);
                }
            }
        }
    }
}
