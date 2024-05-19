using System.ComponentModel.DataAnnotations;

namespace MovieAPI_dotnet.Dtos.Requests.Authentication
{
    public class AuthenticateRequestDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
