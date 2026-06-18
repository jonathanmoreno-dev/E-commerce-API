using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.DTOs.PaymentDTOs
{
    public class PaymentDetailsDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
