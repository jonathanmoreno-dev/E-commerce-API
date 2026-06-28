using Ecommerce.Application.DTOs.CartDTOs;
using Ecommerce.Application.DTOs.CartItemDTOs;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Mappers
{
    internal static class CartMapper
    {
        public static CartListDTO ToListDTO(Cart cart)
        {
            var cartListDTO = new CartListDTO()
            {
                Id = cart.Id,
                User = UserMapper.ToListDTO(cart.User)
            };
            return cartListDTO;
        }
        public static CartDetailsDTO ToDetailsDTO(Cart cart)
        {
            var cartDetailsDTO = new CartDetailsDTO()
            {
                Id = cart.Id,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                CartItems = cart.CartItems.Select(x => CartItemToListDTO(x)).ToList(),
                Total = cart.SubTotal.Value
            };
            return cartDetailsDTO;
        }
        private static CartItemListDTO CartItemToListDTO(CartItem cartItem)
        {
            var cartItemListDTO = new CartItemListDTO()
            {
                Id = cartItem.Id,
                Product = ProductMapper.ToSummaryDTO(cartItem.Product),
                UnitPrice = cartItem.UnitPrice.Value,
                Quantity = cartItem.Quantity.Value,
                SubTotal = cartItem.Quantity.Value * cartItem.UnitPrice.Value
            };
            return cartItemListDTO;
        }
    }
}
