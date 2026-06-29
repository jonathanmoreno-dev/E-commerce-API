using Ecommerce.Application.DTOs.CheckoutDTOs;
using Ecommerce.Application.DTOs.CheckoutItemDTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Mappers
{
    internal static class CheckoutMapper
    {
        public static CheckoutSummaryDTO ToSummaryDTO(Checkout checkout)
        {
            var checkoutSummaryDTO = new CheckoutSummaryDTO()
            {
                Id = checkout.Id,
                CreatedAt = checkout.CreatedAt,
                ExpiresAt = checkout.ExpiresAt,
                Total = checkout.Total.Value,
                PaymentMethod = checkout.PaymentMethod,
                CheckoutItems = checkout.CheckoutItems.Select(x => CheckoutItemToSummaryDTO(x)).ToList()
            };
            return checkoutSummaryDTO;
        }
        public static CheckoutDetailsDTO ToDetailsDTO(Checkout checkout)
        {
            var checkoutDetailsDTO = new CheckoutDetailsDTO()
            {
                Id = checkout.Id,
                FullAddress = BuildFullAddress(checkout.ShippingAddress),
                SubTotal = checkout.SubTotal.Value,
                Total = checkout.Total.Value,
                ShippingCost = checkout.ShippingCost.Value,
                PaymentMethod = checkout.PaymentMethod,
                CheckoutItems = checkout.CheckoutItems.Select(x => CheckoutItemToSummaryDTO(x)).ToList()
            };
            return checkoutDetailsDTO;
        }
        private static CheckoutItemSummaryDTO CheckoutItemToSummaryDTO(CheckoutItem checkoutItem)
        {
            var checkoutItemSummaryDTO = new CheckoutItemSummaryDTO()
            {
                Id = checkoutItem.Id,
                Product = ProductMapper.ToSummaryDTO(checkoutItem.Product),
                Quantity = checkoutItem.Quantity.Value,
                UnitPrice = checkoutItem.UnitPrice.Value
            };
            return checkoutItemSummaryDTO;
        }
        private static string BuildFullAddress(ShippingAddress address)
        {
            return  $"{address.RecipientName.Value} | {address.PhoneNumber.Value} | " +
                    $"{address.Neighborhood}, {address.Street}, {address.Number}, " +
                    $"{address.City} - {address.State}, {address.ZipCode}";
        }
    }
}
