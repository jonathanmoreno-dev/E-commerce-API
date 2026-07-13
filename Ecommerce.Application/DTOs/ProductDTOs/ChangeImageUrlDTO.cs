using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.ProductDTOs
{
    public class ChangeImageUrlDTO
    {
        [Required]
        public ProductImageDTO Image { get; set; } = null!;
        [Required]
        public string NewUrl { get; set; } = "";
    }
}
