using E_commerce_API.src.Application.DTOs.UserDTOs;

namespace E_commerce_API.src.Application.DTOs.OrderDTOs
{
    public class OrderListDTO
    {
        public int Id { get; set; }
        public UserListDTO UserListDTO { get; set; } = null!;
    }
}
