using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities
{
    public class Refund
    {
        public Guid Id { get; private set; }
        public Guid OrderItemId { get; private set; }
        public OrderItem OrderItem { get; private set; } = null!;
        public Quantity Quantity { get; private set; } = null!;
        public DateTime RefundDate { get; private set; }

        private Refund() { }
        public Refund(Quantity quantity)
        {
            Id = Guid.NewGuid();

            ArgumentNullException.ThrowIfNull(quantity);

            Quantity = quantity;
            RefundDate = DateTime.UtcNow;
        }
    }
}
