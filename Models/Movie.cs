namespace MovieAPI_dotnet.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime AiringDate { get; set; }
        public ICollection<UserMovie> UserMovies { get; set; } = new List<UserMovie>();
    }
}
