namespace MovieAPI_dotnet.Dtos
{
    public class IndexDto
    {
        public string? search { get; set; }
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
}
