namespace MovieAPI_dotnet.Dtos
{
    public class PaginationMeta
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }

    public class PaginatedResponse<T>
    {
        public required PaginationMeta Meta { get; set; }
        public required List<T> Data { get; set;}
    }
}
