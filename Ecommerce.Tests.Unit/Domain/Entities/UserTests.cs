using System.IO;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.Entities
{
    public class UserTests
    {
        [Fact]
        public void ShouldCreateUserWithValidValues()
        {
            var fullName = new PersonName("Maria da Silva");
            var email = new Email("user@example.com");
            var phoneNumber = new PhoneNumber("+5538992157062");
            var passwordHash = "hashed-password";

            var user = new User(fullName, email, phoneNumber, passwordHash);

            Assert.NotEqual(Guid.Empty, user.Id);
            Assert.Equal(fullName, user.FullName);
            Assert.Equal(email, user.Email);
            Assert.Equal(phoneNumber, user.PhoneNumber);
            Assert.Equal(passwordHash, user.PasswordHash);
            Assert.Equal(UserRole.Customer, user.Role);
            Assert.Null(user.AvatarImage);
            Assert.Empty(user.ShippingAddresses);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenFullNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new User(
                null!,
                new Email("user@example.com"),
                new PhoneNumber("+5538992157062"),
                "hashed-password"));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenEmailIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new User(
                new PersonName("Maria da Silva"),
                null!,
                new PhoneNumber("+5538992157062"),
                "hashed-password"));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenPhoneNumberIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new User(
                new PersonName("Maria da Silva"),
                new Email("user@example.com"),
                null!,
                "hashed-password"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenPasswordHashIsNullOrWhiteSpace(string passwordHash)
        {
            Assert.Throws<DomainValidationException>(() => new User(
                new PersonName("Maria da Silva"),
                new Email("user@example.com"),
                new PhoneNumber("+5538992157062"),
                passwordHash));
        }

        [Fact]
        public void ShouldChangeName()
        {
            var user = CreateUser();
            var newName = new PersonName("Updated Name");

            user.ChangeName(newName);

            Assert.Equal(newName, user.FullName);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenChangingNameToNull()
        {
            var user = CreateUser();

            Assert.Throws<ArgumentNullException>(() => user.ChangeName(null!));
        }

        [Fact]
        public void ShouldChangeEmail()
        {
            var user = CreateUser();
            var newEmail = new Email("updated@example.com");

            user.ChangeEmail(newEmail);

            Assert.Equal(newEmail, user.Email);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenChangingEmailToNull()
        {
            var user = CreateUser();

            Assert.Throws<ArgumentNullException>(() => user.ChangeEmail(null!));
        }

        [Fact]
        public void ShouldChangePhoneNumber()
        {
            var user = CreateUser();
            var newPhoneNumber = new PhoneNumber("+5549988887824");

            user.ChangePhoneNumber(newPhoneNumber);

            Assert.Equal(newPhoneNumber, user.PhoneNumber);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenChangingPhoneNumberToNull()
        {
            var user = CreateUser();

            Assert.Throws<ArgumentNullException>(() => user.ChangePhoneNumber(null!));
        }

        [Fact]
        public void ShouldChangePasswordHash()
        {
            var user = CreateUser();
            var newPasswordHash = "updated-hashed-password";

            user.ChangePasswordHash(newPasswordHash);

            Assert.Equal(newPasswordHash, user.PasswordHash);
        }

        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenChangingPasswordHashToNullOrWhiteSpace()
        {
            var user = CreateUser();

            Assert.Throws<DomainValidationException>(() => user.ChangePasswordHash(" "));
        }

        [Fact]
        public void ShouldChangeRole()
        {
            var user = CreateUser();

            user.ChangeRole(UserRole.Admin);

            Assert.Equal(UserRole.Admin, user.Role);
        }

        [Fact]
        public void ShouldChangeAvatarImage()
        {
            var user = CreateUser();
            var avatarImage = new AvatarImage("https://example.com/avatar.png");

            user.ChangeAvatarImage(avatarImage);

            Assert.Equal(avatarImage, user.AvatarImage);
        }

        [Fact]
        public void ShouldRemoveAvatarImage()
        {
            var user = CreateUser();
            user.ChangeAvatarImage(new AvatarImage("https://example.com/avatar.png"));

            user.ChangeAvatarImage(null);

            Assert.Null(user.AvatarImage);
        }

        [Fact]
        public void ShouldAddShippingAddress()
        {
            var user = CreateUser();
            var shippingAddress = CreateShippingAddress();

            user.AddShippingAddress(shippingAddress);

            var addedAddress = Assert.Single(user.ShippingAddresses);

            Assert.Equal(shippingAddress, addedAddress);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenAddingShippingAddressNull()
        {
            var user = CreateUser();

            Assert.Throws<ArgumentNullException>(() => user.AddShippingAddress(null!));
        }

        [Fact]
        public void ShouldThrowBusinessRuleExceptionWhenAddingMoreThanFiveShippingAddresses()
        {
            var user = CreateUser();

            for (var i = 0; i < 5; i++)
                user.AddShippingAddress(CreateShippingAddress().WithStreet($"Rua Principal {i}"));

            Assert.Throws<BusinessRuleException>(() => user.AddShippingAddress(CreateShippingAddress().WithStreet("Rua Principal 6")));
        }

        [Fact]
        public void ShouldRemoveShippingAddress()
        {
            var user = CreateUser();
            var shippingAddress = CreateShippingAddress();
            user.AddShippingAddress(shippingAddress);

            user.RemoveShippingAddress(shippingAddress);

            Assert.Empty(user.ShippingAddresses);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenRemovingShippingAddressNull()
        {
            var user = CreateUser();

            Assert.Throws<ArgumentNullException>(() => user.RemoveShippingAddress(null!));
        }

        [Fact]
        public void ShouldThrowNotFoundExceptionWhenRemovingShippingAddressThatDoesNotExist()
        {
            var user = CreateUser();
            user.AddShippingAddress(CreateShippingAddress());

            Assert.Throws<NotFoundException>(() => user.RemoveShippingAddress(
                CreateShippingAddress().WithStreet("Another Street")));
        }

        [Fact]
        public void ShouldReturnNullWhenThereIsNoDefaultShippingAddress()
        {
            var user = CreateUser();

            Assert.Null(user.GetDefaultShippingAddress());
        }

        [Fact]
        public void ShouldReturnTheFirstShippingAddressAsDefault()
        {
            var user = CreateUser();
            var firstAddress = CreateShippingAddress().WithStreet("First Street");
            user.AddShippingAddress(firstAddress);
            user.AddShippingAddress(CreateShippingAddress().WithStreet("Second Street"));

            var defaultAddress = user.GetDefaultShippingAddress();

            Assert.Equal(firstAddress, defaultAddress);
        }

        private static User CreateUser()
        {
            return new User(
                new PersonName("Maria da Silva"),
                new Email("user@example.com"),
                new PhoneNumber("+5538992157062"),
                "hashed-password");
        }

        private static ShippingAddress CreateShippingAddress()
        {
            return new ShippingAddress(
                new PersonName("Maria da Silva"),
                new PhoneNumber("+5538992157062"),
                "Centro",
                "Rua Nova",
                "456",
                "Paraná",
                "Cascavel",
                "42817-000");
        }
    }
}
