namespace MovieAPI_dotnet.Dtos
{
    public class PaginationMeta
    {
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public bool HasPreviousPage { get; set; } = false;
        public bool HasNextPage { get; set; } = false;
    }

    public class PaginatedResponse<T>
    {
        public PaginationMeta? Meta { get; set; }
        public List<T>? Data { get; set;} = new List<T>();
    }
}
