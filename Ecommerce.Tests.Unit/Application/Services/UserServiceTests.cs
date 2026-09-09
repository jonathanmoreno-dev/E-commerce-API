using System.Threading;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.ShippingDTOs;
using Ecommerce.Application.DTOs.UserDTOs;
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
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

        [Fact]
        public async Task ShouldReturnAllUsers()
        {
            var users = new PagedList<User>(
                new[] { CreateUser(), CreateUser() },
                1,
                10,
                2);
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var cancellationToken = new CancellationToken();

            _userRepositoryMock
                .Setup(x => x.GetAllAsync(paginationParams, cancellationToken))
                .ReturnsAsync(users);

            var service = CreateService();

            var result = await service.GetAllAsync(paginationParams, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            _userRepositoryMock.Verify(
                x => x.GetAllAsync(paginationParams, cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task ShouldReturnUsersByRole()
        {
            var role = UserRole.Admin;
            var users = new PagedList<User>(
                new[] { CreateUser(role) },
                1,
                10,
                1);
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var cancellationToken = new CancellationToken();

            _userRepositoryMock
                .Setup(x => x.GetAllByRoleAsync(role, paginationParams, cancellationToken))
                .ReturnsAsync(users);

            var service = CreateService();

            var result = await service.GetAllByRoleAsync(
                role,
                paginationParams,
                cancellationToken);

            Assert.NotNull(result);
            Assert.Single(result.Items);
            _userRepositoryMock.Verify(
                x => x.GetAllByRoleAsync(role, paginationParams, cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task ShouldReturnUserById()
        {
            var user = CreateUser();
            var cancellationToken = new CancellationToken();

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(user.Id, cancellationToken))
                .ReturnsAsync(user);

            var service = CreateService();

            var result = await service.GetByIdAsync(user.Id, cancellationToken);

            Assert.NotNull(result);
            _userRepositoryMock.Verify(
                x => x.GetByIdAsync(user.Id, cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenUserByIdDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(userId, cancellationToken))
                .ReturnsAsync((User?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(
                userId,
                cancellationToken));
        }

        [Fact]
        public async Task ShouldReturnCurrentUser()
        {
            var user = CreateUser();
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(user.Id, cancellationToken))
                .ReturnsAsync(user);

            var service = CreateService();

            var result = await service.GetCurrentAsync(cancellationToken);

            Assert.NotNull(result);
            _userRepositoryMock.Verify(
                x => x.GetByIdAsync(user.Id, cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenCurrentUserDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(userId);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(userId, cancellationToken))
                .ReturnsAsync((User?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetCurrentAsync(cancellationToken));
        }

        [Fact]
        public async Task ShouldUpdateCurrentUser()
        {
            var user = CreateUser();
            var request = new UserUpdateDTO
            {
                FullName = "Updated Name",
                Email = "updated@example.com",
                PhoneNumber = "+5538992157062",
                AvatarImageUrl = "https://example.com/avatar.png"
            };
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(user.Id, cancellationToken))
                .ReturnsAsync(user);

            var service = CreateService();

            var result = await service.UpdateAsync(request, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(new PersonName(request.FullName), user.FullName);
            Assert.Equal(new Email(request.Email), user.Email);
            Assert.Equal(new PhoneNumber(request.PhoneNumber), user.PhoneNumber);
            Assert.Equal(new AvatarImage(request.AvatarImageUrl), user.AvatarImage);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldKeepUserValuesWhenUpdateFieldsAreNull()
        {
            var user = CreateUser();
            var fullName = user.FullName;
            var email = user.Email;
            var phoneNumber = user.PhoneNumber;
            var avatarImage = user.AvatarImage;
            var request = new UserUpdateDTO();
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(user.Id, cancellationToken))
                .ReturnsAsync(user);

            var service = CreateService();

            await service.UpdateAsync(request, cancellationToken);

            Assert.Equal(fullName, user.FullName);
            Assert.Equal(email, user.Email);
            Assert.Equal(phoneNumber, user.PhoneNumber);
            Assert.Equal(avatarImage, user.AvatarImage);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenUpdatingCurrentUserDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var request = new UserUpdateDTO { FullName = "Updated Name" };
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(userId);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(userId, cancellationToken))
                .ReturnsAsync((User?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(
                request,
                cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldChangeCurrentUserPassword()
        {
            var user = CreateUser();
            var currentPasswordHash = user.PasswordHash;
            var request = new ChangePasswordDTO
            {
                CurrentPassword = "CurrentPassword@123",
                NewPassword = "NewPassword@123"
            };
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(user.Id, cancellationToken))
                .ReturnsAsync(user);
            _passwordHasherMock
                .Setup(x => x.VerifyPassword(request.CurrentPassword, currentPasswordHash))
                .Returns(true);
            _passwordHasherMock
                .Setup(x => x.HashPassword(request.NewPassword))
                .Returns("new-hashed-password");

            var service = CreateService();

            await service.ChangePasswordAsync(request, cancellationToken);

            Assert.Equal("new-hashed-password", user.PasswordHash);
            _passwordHasherMock.Verify(
                x => x.VerifyPassword(request.CurrentPassword, currentPasswordHash),
                Times.Once);
            _passwordHasherMock.Verify(x => x.HashPassword(request.NewPassword), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowUnauthorizedExceptionWhenCurrentPasswordIsInvalid()
        {
            var user = CreateUser();
            var request = new ChangePasswordDTO
            {
                CurrentPassword = "WrongPassword@123",
                NewPassword = "NewPassword@123"
            };
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(user.Id, cancellationToken))
                .ReturnsAsync(user);
            _passwordHasherMock
                .Setup(x => x.VerifyPassword(request.CurrentPassword, user.PasswordHash))
                .Returns(false);

            var service = CreateService();

            await Assert.ThrowsAsync<UnauthorizedException>(() => service.ChangePasswordAsync(
                request,
                cancellationToken));

            _passwordHasherMock.Verify(x => x.HashPassword(It.IsAny<string>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenChangingPasswordForMissingUser()
        {
            var userId = Guid.NewGuid();
            var request = new ChangePasswordDTO
            {
                CurrentPassword = "CurrentPassword@123",
                NewPassword = "NewPassword@123"
            };
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(userId);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(userId, cancellationToken))
                .ReturnsAsync((User?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.ChangePasswordAsync(
                request,
                cancellationToken));

            _passwordHasherMock.Verify(
                x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldChangeUserRole()
        {
            var user = CreateUser();
            var cancellationToken = new CancellationToken();

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(user.Id, cancellationToken))
                .ReturnsAsync(user);

            var service = CreateService();

            await service.ChangeRoleAsync(user.Id, UserRole.Manager, cancellationToken);

            Assert.Equal(UserRole.Manager, user.Role);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenChangingRoleForMissingUser()
        {
            var userId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(userId, cancellationToken))
                .ReturnsAsync((User?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.ChangeRoleAsync(
                userId,
                UserRole.Manager,
                cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldAddShippingAddressToCurrentUser()
        {
            var user = CreateUser();
            var request = CreateShippingAddressDTO();
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(user.Id, cancellationToken))
                .ReturnsAsync(user);

            var service = CreateService();

            var result = await service.AddShippingAddressAsync(request, cancellationToken);

            Assert.NotNull(result);
            var address = Assert.Single(user.ShippingAddresses);
            Assert.Equal(request.RecipientName, address.RecipientName.Value);
            Assert.Equal(request.Street, address.Street);
            Assert.Equal(request.Number, address.Number);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowBusinessRuleExceptionWhenAddingSixthShippingAddress()
        {
            var user = CreateUser();
            for (var i = 0; i < 5; i++)
                user.AddShippingAddress(CreateShippingAddress().WithStreet($"Rua Principal {i}"));

            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(user.Id, cancellationToken))
                .ReturnsAsync(user);

            var service = CreateService();

            await Assert.ThrowsAsync<BusinessRuleException>(() => service.AddShippingAddressAsync(
                CreateShippingAddressDTO("Rua Nova"),
                cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldRemoveShippingAddressFromCurrentUser()
        {
            var user = CreateUser();
            var shippingAddress = CreateShippingAddress();
            user.AddShippingAddress(shippingAddress);
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(user.Id, cancellationToken))
                .ReturnsAsync(user);

            var service = CreateService();

            var result = await service.RemoveShippingAddressAsync(
                CreateShippingAddressDTO(),
                cancellationToken);

            Assert.NotNull(result);
            Assert.Empty(user.ShippingAddresses);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenRemovingShippingAddressThatDoesNotExist()
        {
            var user = CreateUser();
            user.AddShippingAddress(CreateShippingAddress());
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(user.Id);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(user.Id, cancellationToken))
                .ReturnsAsync(user);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.RemoveShippingAddressAsync(
                CreateShippingAddressDTO("Another Street"),
                cancellationToken));

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenAddingAddressToMissingUser()
        {
            var userId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();
            SetupCurrentUser(userId);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(userId, cancellationToken))
                .ReturnsAsync((User?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.AddShippingAddressAsync(
                CreateShippingAddressDTO(),
                cancellationToken));
        }

        [Fact]
        public async Task ShouldDeleteUser()
        {
            var user = CreateUser();
            var cancellationToken = new CancellationToken();

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(user.Id, cancellationToken))
                .ReturnsAsync(user);

            var service = CreateService();

            await service.DeleteAsync(user.Id, cancellationToken);

            _userRepositoryMock.Verify(x => x.Remove(user), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionWhenDeletingUserDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(userId, cancellationToken))
                .ReturnsAsync((User?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteAsync(
                userId,
                cancellationToken));

            _userRepositoryMock.Verify(x => x.Remove(It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        private UserService CreateService()
        {
            return new UserService(
                _userRepositoryMock.Object,
                _currentUserServiceMock.Object,
                _passwordHasherMock.Object,
                _unitOfWorkMock.Object);
        }

        private void SetupCurrentUser(Guid userId)
        {
            _currentUserServiceMock
                .SetupGet(x => x.UserId)
                .Returns(userId);
        }

        private static User CreateUser(UserRole role = UserRole.Customer)
        {
            var user = new User(
                new PersonName("Maria da Silva"),
                new Email("user@example.com"),
                new PhoneNumber("+5538992157062"),
                "hashed-password");

            user.ChangeRole(role);

            var cart = new Cart(user.Id);
            SetProperty(user, nameof(User.Cart), cart);
            SetProperty(cart, nameof(Cart.User), user);

            return user;
        }

        private static ShippingAddressDTO CreateShippingAddressDTO(string street = "Rua Principal")
        {
            return new ShippingAddressDTO
            {
                RecipientName = "Exemplo de Nome",
                PhoneNumber = "+5549988887824",
                Neighborhood = "Palmeiras",
                Street = street,
                Number = "820",
                State = "Paraná",
                City = "Foz do Iguaçu",
                ZipCode = "52352-000"
            };
        }

        private static ShippingAddress CreateShippingAddress()
        {
            return new ShippingAddress(
                new PersonName("Exemplo de Nome"),
                new PhoneNumber("+5549988887824"),
                "Palmeiras",
                "Rua Principal",
                "820",
                "Paraná",
                "Foz do Iguaçu",
                "52352-000"
            );
        }

        private static void SetProperty<T>(T target, string propertyName, object value)
        {
            typeof(T).GetProperty(propertyName)!.SetValue(target, value);
        }
    }
}