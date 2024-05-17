namespace MovieAPI_dotnet.Dtos.Responses.Movies
{
    public class MovieDetailResponseDto
    {
        public class UserDto
        {
            public int? Id { get; set; }
            public string? Name { get; set; }
        }
        public class MovieDto
        {
            public int Id { get; set; }
            public required string Title { get; set; }
            public required string Description { get; set; }
            public List<UserDto>? Users { get; set;}
        }
    }
}
