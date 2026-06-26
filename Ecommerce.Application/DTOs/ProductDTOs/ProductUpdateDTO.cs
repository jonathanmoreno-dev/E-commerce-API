using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.ProductDTOs
{
    public class ProductUpdateDTO
    {
        [MaxLength(255)]
        public string? Name { get; set; }
        [MaxLength(400)]
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
    }
}
