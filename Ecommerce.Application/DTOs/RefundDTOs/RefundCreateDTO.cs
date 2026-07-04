using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.RefundDTOs
{
    public class RefundCreateDTO
    {
        public Guid OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public int Quantity { get; set; }
    }
}
