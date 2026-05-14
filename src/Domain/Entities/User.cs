using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class User
    {
        public int UserId { get; private set; }
        public PersonName FullName { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public PhoneNumber PhoneNumber { get; private set; } = null!;
        public string PasswordHash { get; private set; } = "";
        public int CartId { get; private set; }
        public Cart Cart { get; private set; } = null!;
        private List<Order> _orders = new();
        public IReadOnlyCollection<Order> Orders => _orders;
        public bool IsAdmin { get; private set; }
        private User() { }
        public User(string fullName, string email, string phoneNumber, string passwordHash, int cartId)
        {
            CartId = cartId;
            ChangeName(fullName);
            ChangeEmail(email);
            ChangePhoneNumber(phoneNumber);
            ChangePasswordHash(passwordHash);
        }
        public User(string fullName, string email, string phoneNumber, string passwordHash, Cart cart) : this(fullName, email, phoneNumber, passwordHash, cart.CartId)
        {
            Cart = cart;
        }
        public void ChangeName(string fullName)
        {
            FullName = new PersonName(fullName);
        }
        public void ChangeEmail(string email)
        {
            Email = new Email(email);
        }
        public void ChangePhoneNumber(string phoneNumber)
        {
            PhoneNumber = new PhoneNumber(phoneNumber);
        }
        public void ChangePasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }
        public void AddOrder(Order order)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));
            if (_orders.Any(x => x.OrderId == order.OrderId))
                throw new InvalidOperationException($"Order with Id: {order.OrderId} already in user");

            _orders.Add(order);
        }
        public void RemoveOrder(int orderId)
        {
            var order = _orders.FirstOrDefault(x => x.OrderId == orderId);
            if (order is null)
                throw new KeyNotFoundException($"Order with Id: {orderId} was not found");

            _orders.Remove(order);
        }
    }
}
