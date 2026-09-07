using System.Linq;
using System.Threading;
using Ecommerce.Application.DTOs.CheckoutDTOs;
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
    public class CheckoutServiceTests
    {
        private readonly Mock<ICheckoutRepository> _checkoutRepositoryMock = new();
        private readonly Mock<ICartRepository> _cartRepositoryMock = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
        private readonly Mock<IOrderService> _orderServiceMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

        [Fact]
        public async Task ShouldReturnAllActiveCheckouts()
        {
            var checkout = CreateCheckoutWithProduct();
            var checkouts = new PagedList<Checkout>(
                new[] { checkout },
                1,
                10,
                1);
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var cancellationToken = new CancellationToken();

            _checkoutRepositoryMock
                .Setup(x => x.GetAllActiveWithPaymentAttemptsAsync(paginationParams, cancellationToken))
                .ReturnsAsync(checkouts);

            var service = CreateService();

            var result = await service.GetAllActiveAsync(paginationParams, cancellationToken);

            Assert.NotNull(result);
            Assert.Single(result.Items);
            _checkoutRepositoryMock.Verify(
                x => x.GetAllActiveWithPaymentAttemptsAsync(paginationParams, cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task ShouldReturnActiveCheckoutsByUserId()
        {
            var user = CreateUserWithAddress();
            var checkout = CreateCheckoutWithProduct(user);
            var checkouts = new PagedList<Checkout>(
                new[] { checkout },
                1,
                10,
                1);
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var cancellationToken = new CancellationToken();

            _checkoutRepositoryMock
                .Setup(x => x.GetAllActiveWithPaymentAttemptsByUserIdAsync(user.Id, paginationParams, cancellationToken))
                .ReturnsAsync(checkouts);

            var service = CreateService();

            var result = await service.GetAllActiveByUserIdAsync(user.Id, paginationParams,cancellationToken);

            Assert.NotNull(result);
            Assert.Single(result.Items);

            _checkoutRepositoryMock.Verify(
                x => x.GetAllActiveWithPaymentAttemptsByUserIdAsync(
                    user.Id,
                    paginationParams,
                    cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task ShouldReturnCurrentUserActiveCheckouts()
        {
            var user = CreateUserWithAddress();
            var checkout = CreateCheckoutWithProduct(user);
            var checkouts = new PagedList<Checkout>(
                new[] { checkout },
                1,
                10,
                1);
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _checkoutRepositoryMock
                .Setup(x => x.GetAllActiveWithPaymentAttemptsByUserIdAsync(user.Id, paginationParams, cancellationToken))
                .ReturnsAsync(checkouts);

            var service = CreateService();

            var result = await service.GetAllCurrentUserCheckoutsActiveAsync(paginationParams, cancellationToken);

            Assert.NotNull(result);
            Assert.Single(result.Items);
            _checkoutRepositoryMock.Verify(
                x => x.GetAllActiveWithPaymentAttemptsByUserIdAsync(
                    user.Id,
                    paginationParams,
                    cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task ShouldReturnCheckoutById()
        {
            var user = CreateUserWithAddress();
            var checkout = CreateCheckoutWithProduct(user);
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _checkoutRepositoryMock.Setup(x => x.GetByIdAsync(checkout.Id, cancellationToken)).ReturnsAsync(checkout);

            var service = CreateService();

            var result = await service.GetByIdAsync(checkout.Id, cancellationToken);

            Assert.NotNull(result);

            _checkoutRepositoryMock.Verify(x => x.GetByIdAsync(checkout.Id, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenCheckoutByIdDoesNotExist()
        {
            var checkoutId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _checkoutRepositoryMock.Setup(x => x.GetByIdAsync(checkoutId, cancellationToken)).ReturnsAsync((Checkout?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(checkoutId, cancellationToken));
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenCheckoutBelongsToAnotherUser()
        {
            var checkout = CreateCheckoutWithProduct();
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(Guid.NewGuid());

            _checkoutRepositoryMock.Setup(x => x.GetByIdAsync(checkout.Id, cancellationToken)).ReturnsAsync(checkout);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(checkout.Id, cancellationToken));
        }

        [Fact]
        public async Task ShouldCreateCheckout()
        {
            var user = CreateUserWithAddress();
            var product = CreateProduct(10);
            var cart = CreateCart(user, product, 2);
            var cancellationToken = new CancellationToken();
            Checkout? addedCheckout = null;
            SetupCurrentUser(user.Id);

            _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id, cancellationToken)).ReturnsAsync(user);
            _cartRepositoryMock.Setup(x => x.GetByUserIdAsync(user.Id, cancellationToken)).ReturnsAsync(cart);
            _checkoutRepositoryMock
                .Setup(x => x.Add(It.IsAny<Checkout>()))
                .Callback<Checkout>(checkout =>
                {
                    addedCheckout = checkout;
                    SetCheckoutNavigations(checkout, user, product);
                });

            var service = CreateService();

            var result = await service.CreateAsync(cancellationToken);

            Assert.NotNull(result);
            Assert.NotNull(addedCheckout);
            Assert.Equal(user.Id, addedCheckout.UserId);
            Assert.Equal(PaymentMethod.Pix, addedCheckout.PaymentMethod);
            Assert.Equal(new Quantity(2), product.ReservedStock);
            _checkoutRepositoryMock.Verify(x => x.Add(It.IsAny<Checkout>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenCreatingCheckoutUserDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(userId);

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, cancellationToken)).ReturnsAsync((User?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.CreateAsync(cancellationToken));

            _checkoutRepositoryMock.Verify(x => x.Add(It.IsAny<Checkout>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenCreatingCheckoutCartDoesNotExist()
        {
            var user = CreateUserWithAddress();
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id, cancellationToken)).ReturnsAsync(user);
            _cartRepositoryMock.Setup(x => x.GetByUserIdAsync(user.Id, cancellationToken)).ReturnsAsync((Cart?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.CreateAsync(cancellationToken));

            _checkoutRepositoryMock.Verify(x => x.Add(It.IsAny<Checkout>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowBusinessRuleExceptionWhenCreatingCheckoutWithoutShippingAddress()
        {
            var user = CreateUser();
            var product = CreateProduct(10);
            var cart = CreateCart(user, product, 1);
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id, cancellationToken)).ReturnsAsync(user);
            _cartRepositoryMock.Setup(x => x.GetByUserIdAsync(user.Id, cancellationToken)).ReturnsAsync(cart);

            var service = CreateService();

            await Assert.ThrowsAsync<BusinessRuleException>(() => service.CreateAsync(cancellationToken));

            _checkoutRepositoryMock.Verify(x => x.Add(It.IsAny<Checkout>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenCreatingCheckoutProductDoesNotExist()
        {
            var user = CreateUserWithAddress();
            var cart = new Cart(user.Id);
            cart.AddItem(Guid.NewGuid(), new Money(99.90m), new Quantity(1));
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id, cancellationToken)).ReturnsAsync(user);
            _cartRepositoryMock.Setup(x => x.GetByUserIdAsync(user.Id, cancellationToken)).ReturnsAsync(cart);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.CreateAsync(cancellationToken));

            _checkoutRepositoryMock.Verify(x => x.Add(It.IsAny<Checkout>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldUpdateCheckoutPaymentMethodAndShippingAddress()
        {
            var user = CreateUserWithAddress();
            var checkout = CreateCheckoutWithProduct(user);
            var request = new CheckoutUpdateDTO
            {
                PaymentMethod = PaymentMethod.Pix,
                ShippingAddress = new()
                {
                    RecipientName = "Maria da Silva",
                    PhoneNumber = "+5538992157062",
                    Neighborhood = "Centro",
                    Street = "Rua Nova",
                    Number = "456",
                    State = "Foz do Iguaçu",
                    City = "Paraná",
                    ZipCode = "53252-000"
                }
            };
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _checkoutRepositoryMock
                .Setup(x => x.GetByIdWithPaymentAttemptsAsync(checkout.Id, cancellationToken))
                .ReturnsAsync(checkout);

            var service = CreateService();

            var result = await service.UpdateAsync(checkout.Id, request, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(PaymentMethod.Pix, checkout.PaymentMethod);
            Assert.Equal("Rua Nova", checkout.ShippingAddress.Street);
            Assert.Equal("456", checkout.ShippingAddress.Number);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenUpdatingCheckoutDoesNotExist()
        {
            var checkoutId = Guid.NewGuid();
            var request = new CheckoutUpdateDTO { PaymentMethod = PaymentMethod.Pix };
            var cancellationToken = new CancellationToken();

            _checkoutRepositoryMock
                .Setup(x => x.GetByIdWithPaymentAttemptsAsync(checkoutId, cancellationToken))
                .ReturnsAsync((Checkout?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(checkoutId, request, cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenUpdatingCheckoutBelongsToAnotherUser()
        {
            var checkout = CreateCheckoutWithProduct();
            var request = new CheckoutUpdateDTO { PaymentMethod = PaymentMethod.Pix };
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(Guid.NewGuid());

            _checkoutRepositoryMock
                .Setup(x => x.GetByIdWithPaymentAttemptsAsync(checkout.Id, cancellationToken))
                .ReturnsAsync(checkout);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(checkout.Id, request, cancellationToken));
        }

        [Fact]
        public async Task ShouldProcessExpiredCheckouts()
        {
            var user = CreateUserWithAddress();
            var product = CreateProduct(10);
            var checkout = CreateCheckoutWithProduct(user, product, 2);
            product.ReserveStock(new Quantity(2));
            var cancellationToken = new CancellationToken();

            _checkoutRepositoryMock.Setup(x => x.GetAllExpiredNotProcessedAsync(cancellationToken)).ReturnsAsync(new[] { checkout });

            var service = CreateService();

            await service.ProcessExpiredCheckoutsAsync(cancellationToken);

            Assert.Equal(new Quantity(0), product.ReservedStock);
            Assert.NotNull(checkout.ExpirationProcessedAt);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldCreatePaymentAndClearCurrentUserCart()
        {
            var user = CreateUserWithAddress();
            var product = CreateProduct(10);
            var checkout = CreateCheckoutWithProduct(user, product, 1);
            var cart = CreateCart(user, product, 1);
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _checkoutRepositoryMock
                .Setup(x => x.GetByIdWithPaymentAttemptsAsync(checkout.Id, cancellationToken))
                .ReturnsAsync(checkout);
            _cartRepositoryMock
                .Setup(x => x.GetByUserIdAsync(user.Id, cancellationToken))
                .ReturnsAsync(cart);

            var service = CreateService();

            await service.CreatePaymentAsync(checkout.Id, cancellationToken);

            Assert.True(checkout.HasStartedPayment);
            Assert.Empty(cart.CartItems);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenCreatingPaymentCheckoutDoesNotExist()
        {
            var checkoutId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(Guid.NewGuid());

            _checkoutRepositoryMock
                .Setup(x => x.GetByIdWithPaymentAttemptsAsync(checkoutId, cancellationToken))
                .ReturnsAsync((Checkout?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.CreatePaymentAsync(checkoutId, cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenCreatingPaymentCartDoesNotExist()
        {
            var user = CreateUserWithAddress();
            var checkout = CreateCheckoutWithProduct(user);
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _checkoutRepositoryMock
                .Setup(x => x.GetByIdWithPaymentAttemptsAsync(checkout.Id, cancellationToken))
                .ReturnsAsync(checkout);
            _cartRepositoryMock.Setup(x => x.GetByUserIdAsync(user.Id, cancellationToken)).ReturnsAsync((Cart?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.CreatePaymentAsync(checkout.Id, cancellationToken));

            Assert.False(checkout.HasStartedPayment);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldAuthorizePayment()
        {
            var checkout = CreateCheckoutWithPendingPayment();
            var cancellationToken = new CancellationToken();

            _checkoutRepositoryMock
                .Setup(x => x.GetByIdWithPaymentAttemptsAsync(checkout.Id, cancellationToken))
                .ReturnsAsync(checkout);

            var service = CreateService();

            await service.AuthorizePaymentAsync(checkout.Id, cancellationToken);

            Assert.Equal(PaymentStatus.Authorized, checkout.PaymentAttempts.Single().Status);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldCompletePaymentCreateOrderAndRemoveCheckout()
        {
            var user = CreateUserWithAddress();
            var product = CreateProduct(10);
            var checkout = CreateCheckoutWithProduct(user, product, 2);
            product.ReserveStock(new Quantity(2));
            checkout.CreatePayment();
            checkout.AuthorizePayment();
            var cancellationToken = new CancellationToken();

            _checkoutRepositoryMock
                .Setup(x => x.GetByIdWithPaymentAttemptsAsync(checkout.Id, cancellationToken))
                .ReturnsAsync(checkout);

            var service = CreateService();

            await service.CompletePaymentAsync(checkout.Id, cancellationToken);

            Assert.Equal(new Quantity(0), product.ReservedStock);
            Assert.Equal(new Quantity(8), product.Stock);
            Assert.Equal(PaymentStatus.Completed, checkout.PaymentAttempts.Single().Status);

            _checkoutRepositoryMock.Verify(x => x.Remove(checkout), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldFailPayment()
        {
            var checkout = CreateCheckoutWithPendingPayment();
            var cancellationToken = new CancellationToken();

            _checkoutRepositoryMock
                .Setup(x => x.GetByIdWithPaymentAttemptsAsync(checkout.Id, cancellationToken))
                .ReturnsAsync(checkout);

            var service = CreateService();

            await service.FailPaymentAsync(checkout.Id, cancellationToken);

            Assert.Equal(PaymentStatus.Failed, checkout.PaymentAttempts.Single().Status);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldCancelPayment()
        {
            var checkout = CreateCheckoutWithPendingPayment();
            var cancellationToken = new CancellationToken();

            _checkoutRepositoryMock
                .Setup(x => x.GetByIdWithPaymentAttemptsAsync(checkout.Id, cancellationToken))
                .ReturnsAsync(checkout);

            var service = CreateService();

            await service.CancelPaymentAsync(checkout.Id, cancellationToken);

            Assert.Equal(PaymentStatus.Canceled, checkout.PaymentAttempts.Single().Status);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldAbandonPayment()
        {
            var checkout = CreateCheckoutWithPendingPayment();
            var cancellationToken = new CancellationToken();

            _checkoutRepositoryMock
                .Setup(x => x.GetByIdWithPaymentAttemptsAsync(checkout.Id, cancellationToken))
                .ReturnsAsync(checkout);

            var service = CreateService();

            await service.AbandonPaymentAsync(checkout.Id, cancellationToken);

            Assert.Equal(PaymentStatus.Abandoned, checkout.PaymentAttempts.Single().Status);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldDeleteCheckoutAndCancelStockReservation()
        {
            var user = CreateUserWithAddress();
            var product = CreateProduct(10);
            var checkout = CreateCheckoutWithProduct(user, product, 2);
            product.ReserveStock(new Quantity(2));
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _checkoutRepositoryMock.Setup(x => x.GetByIdAsync(checkout.Id, cancellationToken)).ReturnsAsync(checkout);

            var service = CreateService();

            await service.DeleteAsync(checkout.Id, cancellationToken);

            Assert.Equal(new Quantity(0), product.ReservedStock);

            _checkoutRepositoryMock.Verify(x => x.Remove(checkout), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenDeletingCheckoutDoesNotExist()
        {
            var checkoutId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(Guid.NewGuid());

            _checkoutRepositoryMock.Setup(x => x.GetByIdAsync(checkoutId, cancellationToken)).ReturnsAsync((Checkout?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteAsync(checkoutId, cancellationToken));

            _checkoutRepositoryMock.Verify(x => x.Remove(It.IsAny<Checkout>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenDeletingCheckoutBelongsToAnotherUser()
        {
            var checkout = CreateCheckoutWithProduct();
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(Guid.NewGuid());

            _checkoutRepositoryMock.Setup(x => x.GetByIdAsync(checkout.Id, cancellationToken)).ReturnsAsync(checkout);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteAsync(checkout.Id, cancellationToken));

            _checkoutRepositoryMock.Verify(x => x.Remove(It.IsAny<Checkout>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        private CheckoutService CreateService()
        {
            return new CheckoutService(
                _checkoutRepositoryMock.Object,
                _cartRepositoryMock.Object,
                _userRepositoryMock.Object,
                _currentUserServiceMock.Object,
                _orderServiceMock.Object,
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

        private static User CreateUserWithAddress()
        {
            var user = CreateUser();
            user.AddShippingAddress(CreateShippingAddress());
            return user;
        }

        private static Cart CreateCart(User user, Product product, int quantity)
        {
            var cart = new Cart(user.Id);
            cart.AddItem(product.Id, product.Price, new Quantity(quantity));
            typeof(Cart).GetProperty(nameof(Cart.User))!.SetValue(cart, user);

            foreach (var item in cart.CartItems)
            {
                typeof(CartItem).GetProperty(nameof(CartItem.Product))!.SetValue(item, product);
            }

            return cart;
        }

        private static Checkout CreateCheckoutWithProduct(User? user = null, Product? product = null, int quantity = 1)
        {
            user ??= CreateUserWithAddress();
            product ??= CreateProduct(10);

            var checkout = new Checkout(
                user.Id,
                user.GetDefaultShippingAddress() ?? CreateShippingAddress(),
                new Money(30),
                new[] { (product.Id, product.Price, new Quantity(quantity)) });
            checkout.ChangePaymentMethod(PaymentMethod.CreditCard);
            SetCheckoutNavigations(checkout, user, product);

            return checkout;
        }

        private static Checkout CreateCheckoutWithPendingPayment()
        {
            var checkout = CreateCheckoutWithProduct();
            checkout.CreatePayment();
            return checkout;
        }

        private static void SetCheckoutNavigations(Checkout checkout, User user, Product product)
        {
            typeof(Checkout).GetProperty(nameof(Checkout.User))!.SetValue(checkout, user);
            foreach (var item in checkout.CheckoutItems)
            {
                typeof(CheckoutItem).GetProperty(nameof(CheckoutItem.Product))!.SetValue(item, product);
            }
        }

        private static Product CreateProduct(int stock)
        {
            return new Product(
                new ProductName("Product"),
                new ProductShortDescription("Short description"),
                new ProductLongDescription("Long product description"),
                new Money(99.90m),
                new Quantity(stock));
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
                "42141-000"
            );
        }
    }
}
