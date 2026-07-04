using Ecommerce.Application.DTOs.ProductDTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Mappers
{
    internal static class ProductMapper
    {
        public static ProductListDTO ToListDTO(Product product)
        {
            var productListDTO = new ProductListDTO()
            {
                Id = product.Id,
                Name = product.Name.Value,
                Price = product.Price.Value,
                MainImageUrl = product.ProductImages.FirstOrDefault()?.Url ?? ""
            };
            return productListDTO;
        }
        public static ProductSummaryDTO ToSummaryDTO(Product product)
        {
            var productSummaryDTO = new ProductSummaryDTO()
            {
                Id = product.Id,
                Name = product.Name.Value,
                MainImageUrl = product.ProductImages.FirstOrDefault()?.Url ?? ""
            };
            return productSummaryDTO;
        }
        public static ProductDetailsDTO ToDetailsDTO(Product product)
        {
            var productDetailsDTO = new ProductDetailsDTO()
            {
                Id = product.Id,
                Name = product.Name.Value,
                ShortDescription = product.ShortDescription.Value,
                LongDescription = product.LongDescription.Value,
                Price = product.Price.Value,
                Stock = product.Stock.Value,
                ProductImages = product.ProductImages.Select(x => ProductImageToDTO(x)).ToList(),
                Categories = product.Categories.Select(x => CategoryMapper.ToListDTO(x)).ToList()
            };
            return productDetailsDTO;
        }
        private static ProductImageDTO ProductImageToDTO(ProductImage productImage)
        {
            var productImageDTO = new ProductImageDTO()
            {
                Url = productImage.Url,
                Order = productImage.Order
            };
            return productImageDTO;
        }
    }
}
