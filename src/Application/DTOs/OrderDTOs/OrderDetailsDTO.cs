using E_commerce_API.src.Application.DTOs.OrderItemDTOs;
using E_commerce_API.src.Application.DTOs.PaymentDTOs;
using E_commerce_API.src.Application.DTOs.ShippingDTOs;
using E_commerce_API.src.Application.DTOs.UserDTOs;

namespace E_commerce_API.src.Application.DTOs.OrderDTOs
{
    public class OrderDetailsDTO
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "";
        public DateTime CreateAt { get; set; }
        public ShippingListDTO ShippingListDTO { get; set; } = null!;
        public List<PaymentListDTO> PaymentListDTOs { get; set; } = new();
        public List<OrderItemListDTO> OrderItemListDTOs { get; set; } = new();
        public UserListDTO UserListDTO { get; set; } = null!;
    }
}
