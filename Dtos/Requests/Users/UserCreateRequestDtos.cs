using System.ComponentModel.DataAnnotations;

namespace MovieAPI_dotnet.Dtos.Requests.Users
{
    public class UserCreateRequestDtos
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength (100, MinimumLength = 8)]
        public required string Password { get; set; }
    }
}
