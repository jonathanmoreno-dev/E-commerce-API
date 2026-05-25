using E_commerce_API.src.Domain.Enums;

namespace E_commerce_API.src.Application.DTOs.PaymentDTOs
{
    public class PaymentSummaryDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
