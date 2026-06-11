using System.ComponentModel.DataAnnotations;

namespace E_commerce_API.src.Application.DTOs.CartDTOs
{
    public class CartCreateDTO
    {
        [Required]
        public int UserId { get; set; }
    }
}
