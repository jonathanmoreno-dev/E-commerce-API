using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.ProductDTOs
{
    public class ChangeImageOrderDTO
    {
        [Required]
        public ProductImageDTO Image { get; set; } = null!;
        public int NewOrder { get; set; }
    }
}
