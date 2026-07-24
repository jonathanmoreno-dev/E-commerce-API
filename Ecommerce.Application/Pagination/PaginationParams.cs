using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.Pagination
{
    public class PaginationParams
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; init; } = 1;
        [Range(1, 100)]
        public int PageSize { get; init; } = 5;
    }
}
