using Bogus;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Fakers;

public class CheckoutFaker : Faker<Checkout>
{
    public CheckoutFaker()
    {
        CustomInstantiator(f =>
        {
            var items = Enumerable.Range(1, f.Random.Int(1, 5)).Select(_ => (
                f.Random.Guid(),
                new Money(f.Random.Decimal(10, 2000)),
                new Quantity(f.Random.Int(1, 9))
            )).ToList();

            var user = UserFaker.Create();

            return new Checkout(
            user.Id,
            new ShippingAddress(
                user.FullName,
                user.PhoneNumber,
                    f.Address.SecondaryAddress(),
                    f.Address.StreetName(),
                    f.Address.BuildingNumber(),
                    f.Address.City(),
                    f.Address.StateAbbr(),
                    f.Address.ZipCode()),
            new Money(30), // Fixed Value
            items);
        });
    }
    public CheckoutFaker(User user, IEnumerable<Product> productsParameter)
    {
        CustomInstantiator(f =>
        {
            var availableProducts = productsParameter.Where(x => x.Stock.Value - x.ReservedStock.Value > 0).ToList();
            var quantityItemsToAdd = f.Random.Int(1, 5);
            List<(Guid productId, Money unitPrice, Quantity quantity)> items = [];
            for (int i = 0; i < quantityItemsToAdd; i++)
            {
                if (availableProducts.Count == 0)
                    break;

                var product = availableProducts[f.Random.Int(0, availableProducts.Count - 1)];
                availableProducts.Remove(product);

                var quantityAvailable = product.Stock.Value - product.ReservedStock.Value;
                if (quantityAvailable <= 0)
                    continue;

                var quantity = f.Random.Int(1, quantityAvailable);
                items.Add(new(product.Id, product.Price, new Quantity(quantity)));
            }
            if (items.Count == 0)
                return null!;

            var address = user.GetDefaultShippingAddress();
            
            if(address is null)
            {
                address = new ShippingAddress(
                    user.FullName,
                    user.PhoneNumber,
                        f.Address.SecondaryAddress(),
                        f.Address.StreetName(),
                        f.Address.BuildingNumber(),
                        f.Address.City(),
                        f.Address.StateAbbr(),
                        f.Address.ZipCode()
                );
            }

            return new Checkout(user.Id, address, new Money(30), items); 
        });
    }
    public static Checkout CreateFakeCheckout() => new CheckoutFaker().Generate();
    public static Checkout CreateRealCheckout(User user, IEnumerable<Product> products) => new CheckoutFaker(user, products).Generate();
}