using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class Refund
    {
        public int Id { get; private set; }
        public int OrderItemId { get; private set; }
        public OrderItem OrderItem { get; private set; } = null!;
        public Quantity Quantity { get; private set; } = null!;
        public DateTime RefundDate { get; private set; }

        private Refund() { }
        public Refund(Quantity quantity)
        {
            ArgumentNullException.ThrowIfNull(quantity);

            Quantity = quantity;
            RefundDate = DateTime.UtcNow;
        }
    }
}
