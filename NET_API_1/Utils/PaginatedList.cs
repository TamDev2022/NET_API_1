using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace NET_API_1.Utils
{
    public class PaginatedList<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => this.CurrentPage > 1;
        public bool HasNext => this.CurrentPage < TotalPages;
        public IEnumerable<T> Items { get; set; }

        public PaginatedList()
        {
            Items = new List<T>();
        }
        public PaginatedList(List<T> items, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = items.Count;
            TotalPages = (int)Math.Ceiling(items.Count / (double)pageSize);
            CurrentPage = pageNumber;
            PageSize = pageSize;
        }



    }
}
