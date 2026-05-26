using E_commerce_API.src.Application.DTOs.UserDTOs;
using E_commerce_API.src.Domain.Enums;

namespace E_commerce_API.src.Application.DTOs.OrderDTOs
{
    public class OrderListDTO
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
    }
}
