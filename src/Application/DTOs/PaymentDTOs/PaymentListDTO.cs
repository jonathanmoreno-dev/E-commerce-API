using E_commerce_API.src.Domain.Enums;

namespace E_commerce_API.src.Application.DTOs.PaymentDTOs
{
    public class PaymentListDTO
    {
        public int Id { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
