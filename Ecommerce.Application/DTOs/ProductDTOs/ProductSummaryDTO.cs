namespace Ecommerce.Application.DTOs.ProductDTOs
{
    public class ProductSummaryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string MainImageUrl { get; set; } = "";
    }
}
