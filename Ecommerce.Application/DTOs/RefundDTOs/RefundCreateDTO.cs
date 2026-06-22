using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.RefundDTOs
{
    public class RefundCreateDTO
    {
        public int OrderId { get; set; }
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
    }
}
