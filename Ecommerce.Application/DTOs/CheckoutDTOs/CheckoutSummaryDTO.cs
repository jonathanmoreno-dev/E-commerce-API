using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Application.DTOs.CheckoutItemDTOs;
using Ecommerce.Application.DTOs.ProductDTOs;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.DTOs.CheckoutDTOs
{
    public class CheckoutSummaryDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public decimal Total { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<CheckoutItemListDTO> CheckoutItemListDTOs { get; set; } = new();
    }
}
