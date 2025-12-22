namespace backend_disc.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        // sit return the last employeees even if they dont fill out the entire page
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public PaginatedList(List<T> items, int pageIndex, int totalCount, int pageSize)
        {
            Items = items;
            PageIndex = pageIndex;
            TotalCount = totalCount;
            PageSize = pageSize;
        }
    }
} 
