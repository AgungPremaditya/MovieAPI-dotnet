using Microsoft.EntityFrameworkCore;

namespace MovieAPI_dotnet.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public int TotalPages { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize) 
        {
            CurrentPage = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        
            this.AddRange(items);
        }
        
        public bool HasPrevious 
        {
            get { return CurrentPage > 0; }
        }

        public bool HasNext
        {
            get { return CurrentPage < TotalPages; }
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize) 
        {
            var count = await source.CountAsync();
            var items = await source
                .Skip((pageIndex-1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
