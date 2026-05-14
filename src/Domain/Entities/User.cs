namespace E_commerce_API.src.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public bool IsAdmin { get; set; }
    }
}
