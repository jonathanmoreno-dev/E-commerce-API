using Bogus;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Fakers;

public class OrderFaker : Faker<Order>
{
    public OrderFaker()
    {
        CustomInstantiator(f =>
        {
            var items = Enumerable.Range(1, f.Random.Int(1, 5)).Select(_ => (
                f.Random.Guid(),
                new Money(f.Random.Decimal(10, 2000)),
                new Quantity(f.Random.Int(1, 5))
            )).ToList();
            var shippingCost = 30;
            var subtotal = items.Sum(x => x.Item2.Value * x.Item3.Value);
            var totalPaid = subtotal + shippingCost;
            var paymentMethod = (PaymentMethod)f.Random.Int(1, 3);

            return new Order(
            f.Random.Guid(),
            new ShippingAddress(
                new PersonName(f.Person.FullName),
                new PhoneNumber(f.Person.Phone),
                f.Address.SecondaryAddress(),
                f.Address.StreetName(),
                f.Address.BuildingNumber(),
                f.Address.City(),
                f.Address.StateAbbr(),
                f.Address.ZipCode()),
            new Money(shippingCost),
            paymentMethod,
            items,
            new Money(totalPaid));
        });
    }
    public OrderFaker(User user, IEnumerable<Product> productsParameter)
    {
        CustomInstantiator(f =>
        {
            var products = productsParameter.ToList();
            var quantityItemsToAdd = f.Random.Int(1, 5);
            List<(Guid productId, Money unitPrice, Quantity quantity)> items = [];
            for (int i = 0; i < quantityItemsToAdd; i++)
            {
                if (products.Count == 0)
                    break;

                var product = products[f.Random.Int(0, products.Count - 1)];
                products.Remove(product);

                var quantity = f.Random.Int(1, 10);
                items.Add(new(product.Id, product.Price, new Quantity(quantity)));
            }
            if (items.Count == 0)
                return null!;

            var shippingCost = 30;
            var subtotal = items.Sum(x => x.unitPrice.Value * x.quantity.Value);
            var totalPaid = subtotal + shippingCost;
            var address = user.GetDefaultShippingAddress();
            if (address is null)
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
            var paymentMethod = (PaymentMethod)f.Random.Int(1, 3);

            return new Order(
                user.Id,
                address,
                new Money(shippingCost),
                paymentMethod,
                items,
                new Money(totalPaid));
        });
    }
    public static Order CreateFakeOrder() => new OrderFaker().Generate();
    public static Order CreateRealOrder(User user, IEnumerable<Product> products) => new OrderFaker(user, products).Generate();
}