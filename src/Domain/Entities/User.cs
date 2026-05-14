using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public PersonName FullName { get; set; } = null!;
        public Email Email { get; set; } = null!;
        public PhoneNumber Phone { get; set; } = null!;
        public string PasswordHash { get; set; } = "";
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public bool IsAdmin { get; set; }
    }
}
