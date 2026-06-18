using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs.CategoryDTOs
{
    public class CategoryUpdateDTO
    {
        [MaxLength(100)]
        public string? Name { get; set; } = "";
        [Required]
        [MaxLength(400)]
        public string? Description { get; set; } = "";
    }
}
