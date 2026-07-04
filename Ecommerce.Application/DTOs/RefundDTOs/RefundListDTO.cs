namespace Ecommerce.Application.DTOs.RefundDTOs
{
    public class RefundListDTO
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public DateTime RefundDate { get; set; }
    }
}
