using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.DTOs.OrderDTOs
{
    public class OrderListDTO
    {
        public Guid Id { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
    }
}
