using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.ValueObjects;
using Ecommerce.Fakers;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Seeders.EntitySeeders
{
    public class OrderSeeder
    {
        private readonly AppDbContext _appDbContext;
        public OrderSeeder(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task SeedAsync()
        {
            var users = await _appDbContext.Users.ToListAsync();
            var products = await _appDbContext.Products.ToListAsync();

            foreach (var user in users)
            {
                var quantityOrdersToAdd = Random.Shared.Next(1, 8);
                for (int i = 0; i < quantityOrdersToAdd; i++)
                {
                    var order = OrderFaker.CreateRealOrder(user, products);
                    if (order is null)
                        continue;

                    var chance = Random.Shared.Next(1, 101);

                    switch (chance)
                    {
                        case <= 10:
                            order.Cancel();
                            break;
                        case <= 30:
                            order.MarkAsProcessing();
                            break;
                        case <= 45:
                            order.MarkAsProcessing();
                            order.MarkAsShipped();
                            break;
                        case <= 75:
                            order.MarkAsProcessing();
                            order.MarkAsShipped();
                            order.MarkAsInTransit();
                            break;
                        case <= 95:
                            order.MarkAsProcessing();
                            order.MarkAsShipped();
                            order.MarkAsInTransit();
                            order.MarkAsDelivered();
                            break;
                        default:
                            order.MarkAsProcessing();
                            order.MarkAsShipped();
                            order.MarkAsInTransit();
                            order.MarkAsDelivered();
                            var orderItem = order.OrderItems.ElementAt(Random.Shared.Next(0, order.OrderItems.Count));
                            order.RefundItem(orderItem.Id, new Quantity(Random.Shared.Next(1, orderItem.Quantity.Value + 1)));
                            order.MarkAsReturned();
                            break;
                    }
                    _appDbContext.Orders.Add(order);
                }
            }
        }
    }
}
