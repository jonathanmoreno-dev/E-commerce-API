using E_commerce_API.src.Domain.Enums;

namespace E_commerce_API.src.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public Shipping Shipping { get; set; } = null!;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
