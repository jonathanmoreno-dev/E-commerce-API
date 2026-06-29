using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.DTOs.OrderItemDTOs;
using Ecommerce.Application.DTOs.RefundDTOs;
using Ecommerce.Application.DTOs.ShippingDTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Mappers
{
    internal static class OrderMapper
    {
        public static OrderListItemDTO ToListItemDTO(Order order)
        {
            var orderListItemDTO = new OrderListItemDTO()
            {
                OrderId = order.Id,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                OrderItems = order.OrderItems.Select(x => OrderItemToSummaryDTO(x)).ToList()
            };
            return orderListItemDTO;
        }
        public static OrderDetailsDTO ToDetailsDTO(Order order)
        {
            var orderDetailsDTO = new OrderDetailsDTO()
            {
                Id = order.Id,
                Total = order.TotalPaid.Value,
                Status = order.Status,
                ShippingCost = order.ShippingCost.Value,
                CreatedAt = order.CreatedAt,
                Shipping = ShippingToDetailsDTO(order.Shipping),
                OrderItems = order.OrderItems.Select(x => OrderItemToDetailsDTO(x)).ToList()
            };
            return orderDetailsDTO;
        }
        private static OrderItemSummaryDTO OrderItemToSummaryDTO(OrderItem orderItem)
        {
            var orderItemSummaryDTO = new OrderItemSummaryDTO()
            {
                Id = orderItem.Id,
                Product = ProductMapper.ToSummaryDTO(orderItem.Product),
                UnitPrice = orderItem.UnitPrice.Value,
                Quantity = orderItem.Quantity.Value,
                Total = orderItem.UnitPrice.Value * orderItem.Quantity.Value,
                RefundedQuantity = orderItem.Refunds.Sum(x => x.Quantity.Value),
                RefundedTotal = orderItem.Refunds.Sum(x => x.Quantity.Value * orderItem.UnitPrice.Value)
            };
            return orderItemSummaryDTO;
        }
        private static OrderItemDetailsDTO OrderItemToDetailsDTO(OrderItem orderItem)
        {
            var orderItemDetailsDTO = new OrderItemDetailsDTO()
            {
                Id = orderItem.Id,
                Product = ProductMapper.ToSummaryDTO(orderItem.Product),
                UnitPrice = orderItem.UnitPrice.Value,
                Quantity = orderItem.Quantity.Value,
                Total = orderItem.UnitPrice.Value * orderItem.Quantity.Value,
                Refunds = orderItem.Refunds.Select(x => RefundToListDTO(x)).ToList(),
                RefundedTotal = orderItem.Refunds.Sum(x => x.Quantity.Value * orderItem.UnitPrice.Value)
            };
            return orderItemDetailsDTO;
        }
        private static RefundListDTO RefundToListDTO(Refund refund)
        {
            var refundListDTO = new RefundListDTO()
            {
                Id = refund.Id,
                Quantity = refund.Quantity.Value,
                RefundDate = refund.RefundDate
            };
            return refundListDTO;
        }
        private static ShippingDetailsDTO ShippingToDetailsDTO(Shipping shipping)
        {
            var shippingDetailsDTO = new ShippingDetailsDTO()
            {
                Id = shipping.Id,
                RecipientName = shipping.ShippingAddress.RecipientName.Value,
                PhoneNumber = shipping.ShippingAddress.PhoneNumber.Value,
                FullAddress = BuildAddress(shipping.ShippingAddress),
                TrackingCode = shipping.TrackingCode,
                ShippedAt = shipping.ShippedDate,
                DeliveredAt = shipping.DeliveredDate,
                Status = shipping.Status
            };
            return shippingDetailsDTO;
        }
        private static string BuildAddress(ShippingAddress addr)
        {
            return $"{addr.Neighborhood}, {addr.Street}, {addr.Number}, " +
                   $"{addr.City} - {addr.State}, {addr.ZipCode}";
        }
    }
}
