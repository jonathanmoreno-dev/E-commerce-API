using System.ComponentModel.DataAnnotations;

namespace E_commerce_API.src.Application.DTOs.CategoryDTOs
{
    public class CategoryCreateDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = "";
        [Required]
        [MaxLength(400)]
        public string Description { get; set; } = "";
    }
}
