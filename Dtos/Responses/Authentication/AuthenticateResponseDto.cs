using MovieAPI_dotnet.Models;

namespace MovieAPI_dotnet.Dtos.Responses.Authentication
{
    public class AuthenticateResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }


        public AuthenticateResponseDto(User user, string token)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Token = token;
        }
    }
}
