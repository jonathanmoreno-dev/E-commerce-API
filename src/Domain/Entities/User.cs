using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public PersonName FullName { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public PhoneNumber PhoneNumber { get; private set; } = null!;
        public string PasswordHash { get; private set; } = "";
        public int CartId { get; private set; }
        public Cart Cart { get; private set; } = null!;
        private readonly List<ShippingAddress> _shippingAddress = new();
        public IReadOnlyCollection<ShippingAddress> ShippingAddresses => _shippingAddress;
        private readonly List<Checkout> _checkouts = new();
        public IReadOnlyCollection<Checkout> Checkouts => _checkouts;
        private readonly List<Order> _orders = new();
        public IReadOnlyCollection<Order> Orders => _orders;
        public bool IsAdmin { get; private set; }
        public AvatarImage? AvatarImage { get; private set; }
        private User() { }
        public User(PersonName fullName, Email email, PhoneNumber phoneNumber, string passwordHash, int cartId)
        {
            CartId = cartId;
            ChangeName(fullName);
            ChangeEmail(email);
            ChangePhoneNumber(phoneNumber);
            ChangePasswordHash(passwordHash);
        }
        public User(PersonName fullName, Email email, PhoneNumber phoneNumber, string passwordHash, Cart cart) 
            : this(fullName, email, phoneNumber, passwordHash, cart?.Id ?? throw new ArgumentNullException(nameof(cart)))
        {
            Cart = cart;
        }
        public void ChangeName(PersonName fullName)
        {
            ArgumentNullException.ThrowIfNull(fullName);

            FullName = fullName;
        }
        public void ChangeEmail(Email email)
        {
            ArgumentNullException.ThrowIfNull(email);

            Email = email;
        }
        public void ChangePhoneNumber(PhoneNumber phoneNumber)
        {
            ArgumentNullException.ThrowIfNull(phoneNumber);

            PhoneNumber = phoneNumber;
        }
        public void ChangePasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }
        public void ChangeAvatarImage(AvatarImage avatarImage)
        {
            ArgumentNullException.ThrowIfNull(avatarImage);

            AvatarImage = avatarImage;
        }
        public void AddShippingAddress(ShippingAddress shippingAddress)
        {
            ArgumentNullException.ThrowIfNull(shippingAddress);

            _shippingAddress.Add(shippingAddress);
        }
        public void RemoveShippingAddress(ShippingAddress shippingAddress)
        {
            ArgumentNullException.ThrowIfNull(shippingAddress);

            if (!_shippingAddress.Contains(shippingAddress))
                throw new KeyNotFoundException("ShippingAddress was not found");

            _shippingAddress.Remove(shippingAddress);
        }
    }
}
