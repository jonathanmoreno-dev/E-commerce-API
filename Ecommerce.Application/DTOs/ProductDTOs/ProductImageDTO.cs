using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.ProductDTOs
{
    public class ProductImageDTO
    {
        [Required]
        public string Url { get; set; } = "";
        public int Order { get; set; }
    }
}
