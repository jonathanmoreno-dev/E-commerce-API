using System.Linq;
using System.Threading;
using Ecommerce.Application.DTOs.CartDTOs;
using Ecommerce.Application.DTOs.CartItemDTOs;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Application.Pagination;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;
using Moq;

namespace Ecommerce.Tests.Unit.Application.Services
{
    public class CartServiceTests
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock = new();
        private readonly Mock<IProductRepository> _productRepositoryMock = new();
        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

        [Fact]
        public async Task ShouldReturnAllCarts()
        {
            var userId = Guid.NewGuid();
            var cart = CreateCart(userId);
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var cancellationToken = new CancellationToken();
            var carts = new PagedList<Cart>(new[] { cart }, 1, 10, 1);

            _cartRepositoryMock.Setup(x => x.GetAllAsync(paginationParams, cancellationToken)).ReturnsAsync(carts);

            var service = CreateService();

            var result = await service.GetAllAsync(paginationParams, cancellationToken);

            Assert.NotNull(result);
            Assert.Single(result.Items);
            _cartRepositoryMock.Verify(x => x.GetAllAsync(paginationParams, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldReturnCartById()
        {
            var cart = CreateCart(Guid.NewGuid());
            var cancellationToken = new CancellationToken();

            _cartRepositoryMock.Setup(x => x.GetByIdAsync(cart.Id, cancellationToken)).ReturnsAsync(cart);

            var service = CreateService();

            var result = await service.GetByIdAsync(cart.Id, cancellationToken);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenCartByIdDoesNotExist()
        {
            var cartId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _cartRepositoryMock.Setup(x => x.GetByIdAsync(cartId, cancellationToken)).ReturnsAsync((Cart?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(cartId, cancellationToken));
        }

        [Fact]
        public async Task ShouldReturnCartByUserId()
        {
            var userId = Guid.NewGuid();
            var cart = CreateCart(userId);
            var cancellationToken = new CancellationToken();

            _cartRepositoryMock.Setup(x => x.GetByUserIdAsync(userId, cancellationToken)).ReturnsAsync(cart);

            var service = CreateService();

            var result = await service.GetByUserIdAsync(userId, cancellationToken);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenCartByUserIdDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _cartRepositoryMock.Setup(x => x.GetByUserIdAsync(userId, cancellationToken)).ReturnsAsync((Cart?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetByUserIdAsync(userId, cancellationToken));
        }

        [Fact]
        public async Task ShouldReturnCurrentUserCart()
        {
            var userId = Guid.NewGuid();
            var cart = CreateCart(userId);
            var cancellationToken = new CancellationToken();
            SetupCurrentUserCart(userId, cart, cancellationToken);

            var service = CreateService();

            var result = await service.GetCurrentUserCartAsync(cancellationToken);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenCurrentUserCartDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();
            _currentUserServiceMock.SetupGet(x => x.UserId).Returns(userId);
            _cartRepositoryMock.Setup(x => x.GetByUserIdAsync(userId, cancellationToken)).ReturnsAsync((Cart?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetCurrentUserCartAsync(cancellationToken));
        }

        [Fact]
        public async Task ShouldAddItemToCurrentUserCart()
        {
            var userId = Guid.NewGuid();
            var cart = CreateCart(userId);
            var product = CreateProduct(10);
            AddItemWithProduct(cart, product, 1);
            var request = new CartItemCreateDTO
            {
                ProductId = product.Id,
                Quantity = 2
            };
            var cancellationToken = new CancellationToken();
            SetupCurrentUserCart(userId, cart, cancellationToken);
            _productRepositoryMock
                .Setup(x => x.GetByIdAsync(product.Id, cancellationToken))
                .ReturnsAsync(product);
            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(cancellationToken))
                .Callback(() => SetProductNavigation(cart, product));

            var service = CreateService();

            var result = await service.AddItemAsync(request, cancellationToken);

            Assert.NotNull(result);
            Assert.Single(cart.CartItems);
            Assert.Equal(product.Id, cart.CartItems.Single().ProductId);
            Assert.Equal(new Quantity(2), cart.CartItems.Single().Quantity);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenAddingItemProductDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var cart = CreateCart(userId);
            var productId = Guid.NewGuid();
            var request = new CartItemCreateDTO
            {
                ProductId = productId,
                Quantity = 2
            };
            var cancellationToken = new CancellationToken();
            SetupCurrentUserCart(userId, cart, cancellationToken);
            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId, cancellationToken)).ReturnsAsync((Product?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.AddItemAsync(request, cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowBusinessRuleExceptionWhenAddingUnavailableProduct()
        {
            var userId = Guid.NewGuid();
            var cart = CreateCart(userId);
            var product = CreateProduct(1);
            var request = new CartItemCreateDTO
            {
                ProductId = product.Id,
                Quantity = 2
            };
            var cancellationToken = new CancellationToken();
            SetupCurrentUserCart(userId, cart, cancellationToken);
            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);

            var service = CreateService();

            await Assert.ThrowsAsync<BusinessRuleException>(() => service.AddItemAsync(request, cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldRemoveItemFromCurrentUserCart()
        {
            var userId = Guid.NewGuid();
            var cart = CreateCart(userId);
            var product = CreateProduct(10);
            AddItemWithProduct(cart, product, 2);
            var cancellationToken = new CancellationToken();
            SetupCurrentUserCart(userId, cart, cancellationToken);

            var service = CreateService();

            var result = await service.RemoveItemAsync(product.Id, cancellationToken);

            Assert.NotNull(result);
            Assert.Empty(cart.CartItems);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldUpdateItemQuantityInCurrentUserCart()
        {
            var userId = Guid.NewGuid();
            var cart = CreateCart(userId);
            var product = CreateProduct(10);
            AddItemWithProduct(cart, product, 2);
            var request = new CartItemUpdateDTO
            {
                ProductId = product.Id,
                Quantity = 5
            };
            var cancellationToken = new CancellationToken();
            SetupCurrentUserCart(userId, cart, cancellationToken);
            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);

            var service = CreateService();

            var result = await service.UpdateItemAsync(request, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(new Quantity(5), cart.CartItems.Single().Quantity);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenUpdatingItemProductDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var cart = CreateCart(userId);
            var productId = Guid.NewGuid();
            var request = new CartItemUpdateDTO
            {
                ProductId = productId,
                Quantity = 5
            };
            var cancellationToken = new CancellationToken();
            SetupCurrentUserCart(userId, cart, cancellationToken);
            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId, cancellationToken)).ReturnsAsync((Product?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateItemAsync(request, cancellationToken));
        }

        [Fact]
        public async Task ShouldThrowBusinessRuleExceptionWhenUpdatingWithUnavailableQuantity()
        {
            var userId = Guid.NewGuid();
            var cart = CreateCart(userId);
            var product = CreateProduct(3);
            AddItemWithProduct(cart, product, 2);
            var request = new CartItemUpdateDTO
            {
                ProductId = product.Id,
                Quantity = 4
            };
            var cancellationToken = new CancellationToken();
            SetupCurrentUserCart(userId, cart, cancellationToken);
            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, cancellationToken)).ReturnsAsync(product);

            var service = CreateService();

            await Assert.ThrowsAsync<BusinessRuleException>(() => service.UpdateItemAsync(request, cancellationToken));
        }

        [Fact]
        public async Task ShouldClearCurrentUserCart()
        {
            var userId = Guid.NewGuid();
            var cart = CreateCart(userId);
            var product = CreateProduct(10);
            AddItemWithProduct(cart, product, 2);
            var cancellationToken = new CancellationToken();
            SetupCurrentUserCart(userId, cart, cancellationToken);

            var service = CreateService();

            var result = await service.ClearAsync(cancellationToken);

            Assert.NotNull(result);
            Assert.Empty(cart.CartItems);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        private CartService CreateService()
        {
            return new CartService(
                _cartRepositoryMock.Object,
                _productRepositoryMock.Object,
                _currentUserServiceMock.Object,
                _unitOfWorkMock.Object);
        }

        private void SetupCurrentUserCart(Guid userId, Cart cart, CancellationToken cancellationToken)
        {
            _currentUserServiceMock.SetupGet(x => x.UserId).Returns(userId);
            _cartRepositoryMock.Setup(x => x.GetByUserIdAsync(userId, cancellationToken)).ReturnsAsync(cart);
        }

        private static Cart CreateCart(Guid userId)
        {
            var cart = new Cart(userId);
            var user = new User(
                new PersonName("Maria da Silva"),
                new Email("user@example.com"),
                new PhoneNumber("+5538992157062"),
                "hashed-password");

            typeof(Cart).GetProperty(nameof(Cart.User))!.SetValue(cart, user);

            return cart;
        }

        private static void AddItemWithProduct(Cart cart, Product product, int quantity)
        {
            cart.AddItem(product.Id, product.Price, new Quantity(quantity));

            SetProductNavigation(cart, product);
        }

        private static void SetProductNavigation(Cart cart, Product product)
        {
            foreach (var cartItem in cart.CartItems.Where(x => x.ProductId == product.Id))
            {
                typeof(CartItem).GetProperty(nameof(CartItem.Product))!.SetValue(cartItem, product);
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
    }
}
