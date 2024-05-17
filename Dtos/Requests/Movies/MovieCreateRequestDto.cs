using System.ComponentModel.DataAnnotations;

namespace MovieAPI_dotnet.Dtos.Requests.Movies
{
    public class MovieCreateRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public required string Title { get; set; }

        [Required]
        public required string Description { get; set; }
    }
}
