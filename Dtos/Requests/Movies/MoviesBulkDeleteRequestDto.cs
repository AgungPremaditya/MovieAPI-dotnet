using System.ComponentModel.DataAnnotations;

namespace MovieAPI_dotnet.Dtos.Requests.Movies
{
    public class MoviesBulkDeleteRequestDto
    {
        [Required]
        public required List<int> Ids { get; set; }
    }
}
