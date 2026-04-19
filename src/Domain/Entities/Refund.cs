namespace E_commerce_API.src.Domain.Entities
{
    public class Refund
    {
        public int RefundId { get; set; }
        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime RefundDate { get; set; }
    }
}
