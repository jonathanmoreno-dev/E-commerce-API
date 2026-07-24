namespace Ecommerce.Application.Pagination
{
    public class PagedList<T>
    {
        public IReadOnlyCollection<T> Items { get; init; } = new List<T>();
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;

        public PagedList(IReadOnlyCollection<T> items, int pageNumber, int pageSize, int totalItems)
        {
            Items = items;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
        }

        public PagedList<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            return new PagedList<TResult>(Items.Select(selector).ToList(), PageNumber, PageSize, TotalItems);
        }
    }
}
