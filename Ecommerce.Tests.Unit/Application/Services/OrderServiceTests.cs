using System.Linq;
using System.Threading;
using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.DTOs.RefundDTOs;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Pagination;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;
using Moq;

namespace Ecommerce.Tests.Unit.Application.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

        [Fact]
        public async Task ShouldReturnOrdersByUserId()
        {
            var user = CreateUser();
            var order = CreateOrder(user);
            var orders = new PagedList<Order>(
                new[] { order },
                1,
                10,
                1);
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock
                .Setup(x => x.GetAllByUserIdAsync(user.Id, paginationParams, cancellationToken))
                .ReturnsAsync(orders);

            var service = CreateService();

            var result = await service.GetAllByUserIdAsync(user.Id, paginationParams, cancellationToken);

            Assert.NotNull(result);
            Assert.Single(result.Items);
            _orderRepositoryMock.Verify(x => x.GetAllByUserIdAsync(user.Id, paginationParams, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldReturnCurrentUserOrders()
        {
            var user = CreateUser();
            var order = CreateOrder(user);
            var orders = new PagedList<Order>(
                new[] { order },
                1,
                10,
                1);
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _orderRepositoryMock
                .Setup(x => x.GetAllByUserIdAsync(user.Id, paginationParams, cancellationToken))
                .ReturnsAsync(orders);

            var service = CreateService();

            var result = await service.GetAllCurrentUserOrdersAsync(paginationParams, cancellationToken);

            Assert.NotNull(result);
            Assert.Single(result.Items);

            _orderRepositoryMock.Verify(x => x.GetAllByUserIdAsync(user.Id, paginationParams, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldReturnCurrentUserOrdersByStatus()
        {
            var user = CreateUser();
            var order = CreateOrder(user);
            var status = OrderStatus.Paid;
            var orders = new PagedList<Order>(
                new[] { order },
                1,
                10,
                1);
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _orderRepositoryMock
                .Setup(x => x.GetAllByUserIdAndStatusAsync(
                    user.Id,
                    status,
                    paginationParams,
                    cancellationToken))
                .ReturnsAsync(orders);

            var service = CreateService();

            var result = await service.GetAllCurrentUserOrdersByStatusAsync(status, paginationParams, cancellationToken);

            Assert.NotNull(result);
            Assert.Single(result.Items);

            _orderRepositoryMock.Verify(
                x => x.GetAllByUserIdAndStatusAsync(
                    user.Id,
                    status,
                    paginationParams,
                    cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task ShouldReturnOrderById()
        {
            var user = CreateUser();
            var order = CreateOrder(user);
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id, cancellationToken)).ReturnsAsync(order);

            var service = CreateService();

            var result = await service.GetByIdAsync(order.Id, cancellationToken);

            Assert.NotNull(result);

            _orderRepositoryMock.Verify(x => x.GetByIdAsync(order.Id, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenOrderByIdDoesNotExist()
        {
            var orderId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken)).ReturnsAsync((Order?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(orderId, cancellationToken));
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenOrderBelongsToAnotherUser()
        {
            var order = CreateOrder();
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(Guid.NewGuid());

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id, cancellationToken)).ReturnsAsync(order);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(order.Id, cancellationToken));
        }

        [Fact]
        public void ShouldCreateOrderFromCompletedCheckout()
        {
            var checkout = CreateCompletedCheckout();
            Order? addedOrder = null;

            _orderRepositoryMock.Setup(x => x.Add(It.IsAny<Order>())).Callback<Order>(order => addedOrder = order);

            var service = CreateService();

            service.CreateFromCheckout(checkout);

            Assert.NotNull(addedOrder);
            Assert.Equal(checkout.UserId, addedOrder.UserId);
            Assert.Equal(checkout.PaymentMethod, addedOrder.PaymentMethod);
            Assert.Equal(OrderStatus.Paid, addedOrder.Status);
            Assert.Equal(checkout.CompletedPayment!.Amount, addedOrder.TotalPaid);
            Assert.Single(addedOrder.OrderItems);

            _orderRepositoryMock.Verify(x => x.Add(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public void ShouldThrowInvalidOperationExceptionWhenCreatingOrderWithoutCompletedPayment()
        {
            var checkout = CreateCheckout();
            var service = CreateService();

            Assert.Throws<InvalidOperationException>(() => service.CreateFromCheckout(checkout));

            _orderRepositoryMock.Verify(x => x.Add(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task ShouldRefundOrderItem()
        {
            var user = CreateUser();
            var order = CreateOrder(user);
            var orderItem = order.OrderItems.Single();
            var refundCreate = new RefundCreateDTO
            {
                OrderId = order.Id,
                OrderItemId = orderItem.Id,
                Quantity = 1
            };
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id, cancellationToken)).ReturnsAsync(order);

            var service = CreateService();

            var result = await service.RefundItemAsync(refundCreate, cancellationToken);

            Assert.NotNull(result);
            Assert.Single(orderItem.Refunds);
            Assert.Equal(new Quantity(1), orderItem.Refunds.Single().Quantity);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenRefundOrderDoesNotExist()
        {
            var refundCreate = new RefundCreateDTO
            {
                OrderId = Guid.NewGuid(),
                OrderItemId = Guid.NewGuid(),
                Quantity = 1
            };
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(refundCreate.OrderId, cancellationToken)).ReturnsAsync((Order?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.RefundItemAsync(refundCreate, cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenRefundOrderBelongsToAnotherUser()
        {
            var order = CreateOrder();
            var refundCreate = new RefundCreateDTO
            {
                OrderId = order.Id,
                OrderItemId = order.OrderItems.Single().Id,
                Quantity = 1
            };
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(Guid.NewGuid());

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id, cancellationToken)).ReturnsAsync(order);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.RefundItemAsync(refundCreate, cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldSetTrackingCode()
        {
            var order = CreateOrder();
            var trackingCode = "BR123456789SC";
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id, cancellationToken)).ReturnsAsync(order);

            var service = CreateService();

            await service.SetTrackingCodeAsync(order.Id, trackingCode, cancellationToken);

            Assert.Equal(trackingCode, order.Shipping.TrackingCode);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldCancelOrder()
        {
            var order = CreateOrder();
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id, cancellationToken)).ReturnsAsync(order);

            var service = CreateService();

            await service.CancelAsync(order.Id, cancellationToken);

            Assert.Equal(OrderStatus.Canceled, order.Status);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldMarkOrderAsProcessing()
        {
            var order = CreateOrder();
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id, cancellationToken)).ReturnsAsync(order);

            var service = CreateService();

            await service.MarkAsProcessingAsync(order.Id, cancellationToken);

            Assert.Equal(ShippingStatus.Processing, order.Shipping.Status);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldMarkOrderAsShipped()
        {
            var order = CreateOrder();
            order.MarkAsProcessing();
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id, cancellationToken)).ReturnsAsync(order);

            var service = CreateService();

            await service.MarkAsShippedAsync(order.Id, cancellationToken);

            Assert.Equal(OrderStatus.Shipped, order.Status);
            Assert.Equal(ShippingStatus.Shipped, order.Shipping.Status);
            Assert.NotNull(order.Shipping.ShippedDate);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldMarkOrderAsInTransit()
        {
            var order = CreateOrder();
            MarkOrderAsShipped(order);
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id, cancellationToken)).ReturnsAsync(order);

            var service = CreateService();

            await service.MarkAsInTransitAsync(order.Id, cancellationToken);

            Assert.Equal(ShippingStatus.InTransit, order.Shipping.Status);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldMarkOrderAsDelivered()
        {
            var order = CreateOrder();
            MarkOrderAsInTransit(order);
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id, cancellationToken)).ReturnsAsync(order);

            var service = CreateService();

            await service.MarkAsDeliveredAsync(order.Id, cancellationToken);

            Assert.Equal(OrderStatus.Delivered, order.Status);
            Assert.Equal(ShippingStatus.Delivered, order.Shipping.Status);
            Assert.NotNull(order.Shipping.DeliveredDate);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldMarkOrderAsReturned()
        {
            var order = CreateOrder();
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id, cancellationToken)).ReturnsAsync(order);

            var service = CreateService();

            await service.MarkAsReturnedAsync(order.Id, cancellationToken);

            Assert.Equal(ShippingStatus.Returned, order.Shipping.Status);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenSettingTrackingCodeForMissingOrder()
        {
            var orderId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken)).ReturnsAsync((Order?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.SetTrackingCodeAsync(orderId,"BR123456789SC",cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenCancelingMissingOrder()
        {
            var orderId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken)).ReturnsAsync((Order?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.CancelAsync(orderId,cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenProcessingMissingOrder()
        {
            var orderId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken)).ReturnsAsync((Order?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.MarkAsProcessingAsync(orderId,cancellationToken));
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenShippingMissingOrder()
        {
            var orderId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken)).ReturnsAsync((Order?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.MarkAsShippedAsync(orderId,cancellationToken));
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenInTransitOrderIsMissing()
        {
            var orderId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken)).ReturnsAsync((Order?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.MarkAsInTransitAsync(orderId, cancellationToken));
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenDeliveredOrderIsMissing()
        {
            var orderId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken)).ReturnsAsync((Order?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.MarkAsDeliveredAsync(orderId,cancellationToken));
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenReturnedOrderIsMissing()
        {
            var orderId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken)).ReturnsAsync((Order?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.MarkAsReturnedAsync(orderId, cancellationToken));
        }

        private OrderService CreateService()
        {
            return new OrderService(
                _orderRepositoryMock.Object,
                _currentUserServiceMock.Object,
                _unitOfWorkMock.Object);
        }

        private void SetupCurrentUser(Guid userId)
        {
            _currentUserServiceMock.SetupGet(x => x.UserId).Returns(userId);
        }

        private static User CreateUser()
        {
            return new User(
                new PersonName("Maria da Silva"),
                new Email("user@example.com"),
                new PhoneNumber("+5538992157062"),
                "hashed-password");
        }

        private static Order CreateOrder(User? user = null, Product? product = null)
        {
            user ??= CreateUser();
            product ??= CreateProduct();

            var order = new Order(
                user.Id,
                CreateShippingAddress(),
                new Money(30),
                PaymentMethod.Pix,
                new[] { (product.Id, product.Price, new Quantity(1)) },
                new Money(129.90m));

            typeof(Order).GetProperty(nameof(Order.User))!.SetValue(order, user);
            foreach (var item in order.OrderItems)
            {
                typeof(OrderItem).GetProperty(nameof(OrderItem.Product))!.SetValue(item, product);
            }

            return order;
        }

        private static Checkout CreateCheckout()
        {
            var user = CreateUser();
            var product = CreateProduct();
            var checkout = new Checkout(
                user.Id,
                CreateShippingAddress(),
                new Money(30),
                new[] { (product.Id, product.Price, new Quantity(1)) });
            checkout.ChangePaymentMethod(PaymentMethod.Pix);

            foreach (var item in checkout.CheckoutItems)
            {
                typeof(CheckoutItem).GetProperty(nameof(CheckoutItem.Product))!.SetValue(item, product);
            }

            return checkout;
        }

        private static Checkout CreateCompletedCheckout()
        {
            var checkout = CreateCheckout();
            checkout.CreatePayment();
            checkout.AuthorizePayment();
            checkout.CompletePayment();
            return checkout;
        }

        private static Product CreateProduct()
        {
            return new Product(
                new ProductName("Product"),
                new ProductShortDescription("Short description"),
                new ProductLongDescription("Long product description"),
                new Money(99.90m),
                new Quantity(10));
        }

        private static ShippingAddress CreateShippingAddress()
        {
            return new ShippingAddress(
                new PersonName("Exemplo de Nome"),
                new PhoneNumber("+5549988887824"),
                "Palmeiras",
                "Centro",
                "820",
                "Paraná",
                "Foz do Iguaçu",
                "87852-000"
            );
        }

        private static void MarkOrderAsShipped(Order order)
        {
            order.MarkAsProcessing();
            order.SetTrackingCode("BR123456789SC");
            order.MarkAsShipped();
        }

        private static void MarkOrderAsInTransit(Order order)
        {
            MarkOrderAsShipped(order);
            order.MarkAsInTransit();
        }
    }
}
