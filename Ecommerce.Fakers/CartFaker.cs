using Bogus;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Fakers;

public class CartFaker : Faker<Cart>
{
    public CartFaker()
    {
        CustomInstantiator(f =>
        {
            var cart = new Cart(f.Random.Guid());
            var count = f.Random.Int(1, 5);

            for (int i = 0; i < count; i++)
            {
                cart.AddItem(f.Random.Guid(), new Money(f.Random.Decimal(10, 2000)), new Quantity(f.Random.Int(1, 9)));
            }
            return cart;
        });
    }
    public CartFaker(Guid userId, IEnumerable<Product> productsParameter)
    {
        CustomInstantiator(f =>
        {
            var cart = new Cart(userId);
            var products = productsParameter.Where(x => x.Stock.Value > 0).ToList();
            var quantityItemsToAdd = f.Random.Int(1, 5);
            for (int i = 0; i < quantityItemsToAdd; i++)
            {
                if (products.Count == 0)
                    break;

                var product = products[f.Random.Int(0, products.Count - 1)];

                products.Remove(product);

                cart.AddItem(product.Id, product.Price, new Quantity(f.Random.Int(1, product.Stock.Value)));
            }
            return cart;
        });
    }
    public static Cart CreateFakeCart() => new CartFaker().Generate();
    public static Cart CreateRealCart(Guid userId, IEnumerable<Product> products) => new CartFaker(userId, products).Generate();
}