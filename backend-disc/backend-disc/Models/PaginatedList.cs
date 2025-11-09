using Microsoft.EntityFrameworkCore;

namespace backend_disc.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public PaginatedList(List<T> items, int pageIndex, int totalCount)
        {
            Items = items;
            PageIndex = pageIndex;
            TotalCount = totalCount;
        }


    }
}
