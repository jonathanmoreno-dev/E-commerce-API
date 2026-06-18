using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.DTOs.PaymentDTOs
{
    public class PaymentListDTO
    {
        public int Id { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
