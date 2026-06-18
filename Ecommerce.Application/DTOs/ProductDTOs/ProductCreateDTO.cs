using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.ProductDTOs
{
    public class ProductCreateDTO
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = "";
        [Required]
        [MaxLength(400)]
        public string ShortDescription { get; set; } = "";
        [Required]
        public string LongDescription { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
